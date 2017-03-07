using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

namespace Client
{
	public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
	{
		static ILog _logger = LogManager.GetLogger<PlaceOrderHandler>();

		public Task Handle(PlaceOrder message, IMessageHandlerContext context)
		{
			_logger.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

			// This is normally where some business logic would occur

			//throw new Exception("BOOM!!!!!");

			var orderPlaced = new OrderPlaced
			{
				OrderId = message.OrderId
			};

			return context.Publish(orderPlaced);
		}
	}
}