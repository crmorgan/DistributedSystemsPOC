using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace Client
{
	internal class Program
	{
		static ILog _logger = LogManager.GetLogger<Program>();
	
		private static void Main(string[] args)
		{
			AsyncMain().GetAwaiter().GetResult();
		}

		private static async Task AsyncMain()
		{
			Console.Title = "Client";

			var endpointConfiguration = new EndpointConfiguration("ClientUI");
			var transport = endpointConfiguration.UseTransport<MsmqTransport>();
			var routing = transport.Routing();
			routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

			endpointConfiguration.UseSerialization<JsonSerializer>();
			endpointConfiguration.UsePersistence<InMemoryPersistence>();
			endpointConfiguration.SendFailedMessagesTo("error");
			endpointConfiguration.EnableInstallers();

			var endpointInstance = await Endpoint.Start(endpointConfiguration)
				.ConfigureAwait(false);

			try
			{
				await RunLoop(endpointInstance);
			}
			finally
			{
				await endpointInstance.Stop()
					.ConfigureAwait(false);
			}
		}


		static async Task RunLoop(IEndpointInstance endpointInstance)
		{
			while (true)
			{
				_logger.Info("Press 'P' to place an order, or 'Q' to quit.");
				var key = Console.ReadKey();
				Console.WriteLine();

				switch (key.Key)
				{
					case ConsoleKey.P:

						var command = new PlaceOrder
						{
							OrderId = Guid.NewGuid(),
							Product = "The Walking Dead: Season One"
						};

						// Send the command to the local endpoint
						_logger.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");

						await endpointInstance.Send(command).ConfigureAwait(false);

						break;

					case ConsoleKey.Q:
						return;

					default:
						_logger.Info("Unknown input. Please try again.");
						break;
				}
			}
		}
	}
}