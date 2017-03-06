using System;
using NServiceBus;

namespace Shared
{
	public class OrderBilled : IEvent
	{
		public Guid OrderId { get; set; }
	}
}