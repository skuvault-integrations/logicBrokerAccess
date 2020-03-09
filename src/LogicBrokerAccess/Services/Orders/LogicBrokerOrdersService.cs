using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Models;
using LogicBrokerAccess.Shared;
using Netco.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LogicBrokerAccess.Services.Orders
{
	public class LogicBrokerOrdersService : BaseService, ILogicBrokerOrdersService
	{
		public LogicBrokerOrdersService( LogicBrokerConfig config, LogicBrokerCredentials credentials ) : base( credentials, config )
		{ }

		public async Task< IEnumerable< Order > > GetOrderDetailsAsync( DateTime startDateUtc, DateTime endDateUtc, CancellationToken token, Mark mark )
		{
			var command = new GetOrdersReadyCommand( base.Config.ApiBaseUrl, base.Credentials.SubscriptionKey, startDateUtc, endDateUtc );
			var orders = new List< Order >();
			try
			{
				var response = await base.GetAsync< LogicBrokerOrderRecords >( command, token, mark ).ConfigureAwait( false );
				if( response?.Records != null ) 
				{
					orders = response.Records.Select( r => r.ToSvOrder() ).ToList();
				}
			}
			catch ( Exception ex )
			{
				LogicBrokerLogger.LogTrace( ex, "message" );
			}

			return orders;
		}
	}
}
