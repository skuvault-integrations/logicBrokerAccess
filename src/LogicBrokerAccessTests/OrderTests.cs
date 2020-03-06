using NUnit.Framework;
using LogicBrokerAccess.Services.Orders;
using LogicBrokerAccess;
using System;
using System.Threading;
using Netco.Logging;
using FluentAssertions;
using System.Linq;

namespace LogicBrokerAccessTests
{
	[ TestFixture ]
	public class OrderTests : BaseTest
	{
		private ILogicBrokerOrdersService _ordersService;

		[ SetUp ]
		public void Init()
		{
			_ordersService = new LogicBrokerFactory( this.Config ).CreateOrdersService( this.Credentials );
		}

		[ Test ]
		public void GetReadyOrders()
		{
			var mark = Mark.Blank();

			var orders = _ordersService.GetOrderDetailsAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, mark ).Result;

			orders.Count().Should().NotBe( 0 );
		}
	}
}
