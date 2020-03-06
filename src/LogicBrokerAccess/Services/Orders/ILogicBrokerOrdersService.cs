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
		Task< IEnumerable< Order > > GetOrderDetailsAsync( DateTime startDateUtc, DateTime endDateUtc, CancellationToken none, Mark mark );
	}
}
