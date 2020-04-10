using FluentAssertions;
using LogicBrokerAccess.Commands;
using NUnit.Framework;

namespace LogicBrokerAccessTests
{
	[ TestFixture ]
	public class PostInventoryBroadcastCommandTests
	{
		[ Test ]
		public void CreateInventoryUpdateCsv()
		{
			var sku = "testSku";
			var quantity = 12;
			var sku2 = "testSku2";
			var quantity2 = 6;
			var inventoryItems = new []
			{ 
				new LogicBrokerAccess.Models.LogicBrokerInventoryItem
				{ 
					SupplierSKU = sku,
					Quantity = quantity
				},
				new LogicBrokerAccess.Models.LogicBrokerInventoryItem
				{
					SupplierSKU = sku2,
					Quantity = quantity2
				}
			};

			var command = new PostInventoryBroadcastCommand( "random", "random", inventoryItems );

			command.Payload.Should().Be( "SupplierSKU,Quantity\r\n" + 
				$"{sku},{quantity}\r\n" + 
				$"{sku2},{quantity2}\r\n" );
		}
	}
}
