using System;
using NServiceBus;

namespace Shared
{
	public class PlaceOrder : ICommand
	{
		public Guid OrderId { get; set; }
		public string Product { get; set; }
	}
}