using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Sales
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			AsyncMain().GetAwaiter().GetResult();
		}

		private static async Task AsyncMain()
		{
			Console.Title = "Sales";

			var endpointConfiguration = new EndpointConfiguration("Sales");

			var transport = endpointConfiguration.UseTransport<MsmqTransport>();

			endpointConfiguration.UseSerialization<JsonSerializer>();
			endpointConfiguration.UsePersistence<InMemoryPersistence>();
			endpointConfiguration.SendFailedMessagesTo("error");
			endpointConfiguration.EnableInstallers();

			var recoverability = endpointConfiguration.Recoverability();

			recoverability.Immediate(
				immediate => { immediate.NumberOfRetries(1); });

			recoverability.Delayed(
				delayed =>
				{
					delayed.NumberOfRetries(3);
					delayed.TimeIncrease(TimeSpan.FromSeconds(3));
				});

			var endpointInstance = await Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false);

			Console.WriteLine("Press Enter to exit.");
			Console.ReadLine();

			await endpointInstance.Stop()
				.ConfigureAwait(false);
		}
	}
}