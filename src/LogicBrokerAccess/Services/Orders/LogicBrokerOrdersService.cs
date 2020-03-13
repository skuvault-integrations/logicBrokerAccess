using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Models;
using LogicBrokerAccess.Shared;
using LogicBrokerAccess.Throttling;
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
		public LogicBrokerOrdersService( LogicBrokerConfig config, LogicBrokerCredentials credentials, int pageSize ) 
			: base( credentials, config, pageSize )
		{ }

		public async Task< IEnumerable< Order > > GetOrderDetailsAsync( DateTime startDateUtc, DateTime endDateUtc, CancellationToken token, Mark mark )
		{
			var orders = new List< Order >();
			try
			{
				orders = await CollectOrdersFromAllPages( startDateUtc, endDateUtc, mark, token );
			}
			catch ( Exception ex )
			{
				LogicBrokerLogger.LogTrace( ex, "message" );
			}

			return orders;
		}

		private async Task< List< Order > > CollectOrdersFromAllPages( DateTime startDateUtc, DateTime endDateUtc, Mark mark, CancellationToken token )
		{
			var orders = new List< Order >();
			LogicBrokerOrderResponse response;
			var paging = new Paging( base.PageSize );
			LogicBrokerCommand command = new GetOrdersCommand( base.Config.DomainUrl, base.Credentials.SubscriptionKey, startDateUtc, endDateUtc, base.Config.ThrottlingMaxRetryAttempts, paging );
			do
			{
				response = await base.GetAsync< LogicBrokerOrderResponse >( command, token, mark ).ConfigureAwait( false );
				if( response?.Records != null )
				{
					orders.AddRange( response.Records.Select( r => r.ToSvOrder() ).ToList() );
				}
				command.UpdateCurrentPage( response?.CurrentPage + 1 );
			} while( command.Paging.CurrentPage < response?.TotalPages );

			return orders;
		}
	}
}
