using LogicBrokerAccess.Models;
using Netco.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogicBrokerAccess.Services.Orders
{
	public interface ILogicBrokerOrdersService
	{
		IEnumerable< Order > GetOrdersByDate( DateTime startDateUtc, DateTime endDateUtc, CancellationToken token, Mark mark );

		Task< IEnumerable< Order > > GetOrdersByDateAsync( DateTime startDateUtc, DateTime endDateUtc, CancellationToken none, Mark mark );

		Task< IEnumerable< string > > AcknowledgeOrdersAsync( IEnumerable< string > logicBrokerKeys, CancellationToken token, Mark mark );
	}
}
