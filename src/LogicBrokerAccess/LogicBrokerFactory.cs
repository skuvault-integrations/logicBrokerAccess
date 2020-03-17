using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
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

		public ILogicBrokerOrdersService CreateOrdersService( LogicBrokerCredentials credentials )
		{
			return new LogicBrokerOrdersService( this.Config, credentials, Paging.DefaultPageSize );
		}

		public ILogicBrokerOrdersService CreateOrdersService( LogicBrokerCredentials credentials, int pageSize )
		{
			return new LogicBrokerOrdersService( this.Config, credentials, pageSize );
		}
	}
}
