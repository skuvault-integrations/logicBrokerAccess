using NUnit.Framework;
using LogicBrokerAccess.Services.Orders;
using LogicBrokerAccess;
using System;
using System.Threading;
using Netco.Logging;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

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
			var ordersServiceWithPageSize1 = logicBrokerFactory.CreateOrdersService( this.Credentials, 1 );

			var orders = ordersServiceWithPageSize1.GetOrderDetailsAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, Mark.Blank() ).Result;

			orders.Count().Should().BeGreaterThan( 1 );
		}

		[ Test ]
		public void AcknowledgeOrdersAsync()
		{
			var ordersServiceWithPageSize1 = logicBrokerFactory.CreateOrdersService( this.Credentials, 1 );
			var orderLogicBrokerKeys = new List< string > { "454528", "454511" };

			var acknowledgedOrders = ordersServiceWithPageSize1.AcknowledgeOrdersAsync( orderLogicBrokerKeys, CancellationToken.None, Mark.Blank() ).Result;

			acknowledgedOrders.Count().Should().BeGreaterThan( 1 );
			acknowledgedOrders.Any( o => o == orderLogicBrokerKeys[ 0 ] ).Should().BeTrue();
		}
	}
}
