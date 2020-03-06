using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Services.Orders;

namespace LogicBrokerAccess
{
	public interface ILogicBrokerFactory
	{
		ILogicBrokerOrdersService CreateOrdersService( LogicBrokerCredentials credentials );
	}
}
