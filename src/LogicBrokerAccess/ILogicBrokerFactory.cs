using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Services.Inventory;
using LogicBrokerAccess.Services.Orders;

namespace LogicBrokerAccess
{
	public interface ILogicBrokerFactory
	{
		ILogicBrokerOrdersService CreateOrdersService( LogicBrokerCredentials credentials, int pageSize = Paging.DefaultPageSize );

		ILogicBrokerInventoryService CreateInventoryService( LogicBrokerCredentials credentials );
	}
}
