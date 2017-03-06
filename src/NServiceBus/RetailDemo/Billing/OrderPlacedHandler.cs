using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace Billing
{
	public class OrderPlacedHandler : IHandleMessages<OrderPlaced>

	{
		private static ILog _logger = LogManager.GetLogger<OrderPlacedHandler>();

		public Task Handle(OrderPlaced message, IMessageHandlerContext context)
		{
			_logger.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Charging credit card...");

			var orderPlaced = new OrderPlaced
			{
				OrderId = message.OrderId
			};

			return context.Publish(orderPlaced);
		}
	}
}