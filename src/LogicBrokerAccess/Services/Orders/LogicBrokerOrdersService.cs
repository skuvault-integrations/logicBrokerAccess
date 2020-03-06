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
			var command = new GetOrdersReadyCommand( base.Config.ApiBaseUrl, base.Credentials.SubscriptionKey );

			try
			{
				var response = await base.GetAsync< LogicBrokerOrderRecords >( command, token, mark ).ConfigureAwait( false );
				//TODO GUARD-451 If an error then throw the appropriate Exception
				if( response?.Records != null ) 
				{
					//TODO GUARD-451 Filter by dates
					return response.Records.Select( r => r.ToSvOrder() );
				}
			}
			catch ( Exception ex )
			{
				//TODO GUARD-451 Apparently only skipped exceptions are here
				LogicBrokerLogger.LogTrace( ex, "message" );
			}

			return null;
		}
	}
}
