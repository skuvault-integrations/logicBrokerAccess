using NUnit.Framework;
using LogicBrokerAccess.Services.Orders;
using LogicBrokerAccess;
using System;
using System.Threading;
using Netco.Logging;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;
using LogicBrokerAccess.Exceptions;
using System.Threading.Tasks;

namespace LogicBrokerAccessTests
{
	[ TestFixture ]
	public class OrdersServiceTests : BaseTest
	{
		private ILogicBrokerOrdersService ordersService;
		private ILogicBrokerFactory logicBrokerFactory;

		[ SetUp ]
		public void Init()
		{
			logicBrokerFactory = new LogicBrokerFactory( this.Config );
			ordersService = logicBrokerFactory.CreateOrdersService( this.Credentials );
		}

		[ Test ]
		public void GetOrdersByDateAsync()
		{
			var orders = ordersService.GetOrdersByDateAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, Mark.Blank() ).Result;

			orders.Count().Should().NotBe( 0 );
		}

		[ Test ]
		public async Task GetOrdersByDateAsync_InvalidCredentials_ShouldThrow()
		{
			var invalidCreds = new LogicBrokerAccess.Configuration.LogicBrokerCredentials( "invalid credentials" );
			var service = logicBrokerFactory.CreateOrdersService( invalidCreds );

			try
			{
				await service.GetOrdersByDateAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, Mark.Blank() );
			}
			catch ( Exception ex )
			{
				Assert.IsTrue( ex.InnerException is LogicBrokerException );
			}
		}

		[ Test ]
		public void CollectOrdersFromAllPages()
		{
			var ordersServiceWithPageSize1 = logicBrokerFactory.CreateOrdersService( this.Credentials, 1 );

			var orders = ordersServiceWithPageSize1.GetOrdersByDateAsync( DateTime.UtcNow.AddMonths( -3 ), DateTime.UtcNow, CancellationToken.None, Mark.Blank() ).Result;

			orders.Count().Should().BeGreaterThan( 1 );
		}

		[ Test ]
		[ Ignore("Enter valid unacknowledged LogicBrokerKey's") ]
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
