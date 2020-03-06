using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Services.Orders;

namespace LogicBrokerAccess
{
	public class LogicBrokerFactory
	{
		private LogicBrokerConfig Config;

		public LogicBrokerFactory( LogicBrokerConfig config )
		{
			this.Config = config;
		}

		public ILogicBrokerOrdersService CreateOrdersService( LogicBrokerCredentials credentials )
		{
			return new LogicBrokerOrdersService( this.Config, credentials );
		}
	}
}
