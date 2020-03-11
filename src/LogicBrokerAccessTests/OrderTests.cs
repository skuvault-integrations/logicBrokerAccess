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
		private ILogicBrokerOrdersService ordersService;
		private LogicBrokerFactory logicBrokerFactory;

		[ SetUp ]
		public void Init()
		{
			logicBrokerFactory = new LogicBrokerFactory( this.Config );
			ordersService = logicBrokerFactory.CreateOrdersService( this.Credentials );
		}

		[ Test ]
		public void GetOrderDetails()
		{
			var orders = ordersService.GetOrderDetailsAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, Mark.Blank() ).Result;

			orders.Count().Should().NotBe( 0 );
		}

		[ Test ]
		public void CollectOrdersFromAllPages()
		{
			int pageSize = 1;
			ILogicBrokerOrdersService ordersServiceWithPageSize1 = logicBrokerFactory.CreateOrdersService( this.Credentials, pageSize );

			var orders = ordersServiceWithPageSize1.GetOrderDetailsAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, Mark.Blank() ).Result;

			orders.Count().Should().NotBe( 0 );
		}
	}
}
