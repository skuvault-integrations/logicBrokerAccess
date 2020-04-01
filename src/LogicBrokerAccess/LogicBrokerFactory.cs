using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Services.Inventory;
using LogicBrokerAccess.Services.Orders;

namespace LogicBrokerAccess
{
	public class LogicBrokerFactory : ILogicBrokerFactory
	{
		private LogicBrokerConfig Config;

		public LogicBrokerFactory( LogicBrokerConfig config )
		{
			this.Config = config;
		}

		public ILogicBrokerOrdersService CreateOrdersService( LogicBrokerCredentials credentials, int pageSize = Paging.DefaultPageSize )
		{
			return new LogicBrokerOrdersService( this.Config, credentials, pageSize );
		}

		public ILogicBrokerInventoryService CreateInventoryService( LogicBrokerCredentials credentials )
		{
			return new LogicBrokerInventoryService( this.Config, credentials );
		}
	}
}
