using FluentAssertions;
using LogicBrokerAccess;
using LogicBrokerAccess.Services.Inventory;
using Netco.Logging;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogicBrokerAccessTests
{
	[ TestFixture ]
	public class InventoryServiceTests : BaseTest
	{
		private ILogicBrokerInventoryService inventoryService;
		private ILogicBrokerFactory logicBrokerFactory;

		[ SetUp ]
		public void Init()
		{
			logicBrokerFactory = new LogicBrokerFactory( Config );
			inventoryService = logicBrokerFactory.CreateInventoryService( Credentials );
		}

		[ Test ]
		public async Task UpdatInventoryAsync()
		{
			var inventoryItems = new []
			{ 
				new LogicBrokerAccess.Models.LogicBrokerInventoryItem
				{ 
					SupplierSKU = "TESTSKU4",
					Quantity = 222
				},
				new LogicBrokerAccess.Models.LogicBrokerInventoryItem
				{
					SupplierSKU = "TESTSKU5",
					Quantity = 333
				}
			};

			var result = await inventoryService.UpdateInventoryAsync( inventoryItems, CancellationToken.None, Mark.Blank() );

			result.Should().NotBeNullOrWhiteSpace();
		}
	}
}
