using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Models;
using LogicBrokerAccess.Shared;
using Netco.Extensions;
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
			: base( config, credentials, pageSize )
		{ }

		public IEnumerable< Order > GetOrdersByDate( DateTime startDateUtc, DateTime endDateUtc, CancellationToken token, Mark mark )
		{
			if ( mark == null )
				mark = Mark.CreateNew();

			List< Order > orders;
			try
			{
				orders = CollectOrdersFromAllPagesAsync( startDateUtc, endDateUtc, token, mark ).Result;
			}
			catch ( Exception ex )
			{
				LogicBrokerLogger.LogTrace( ex.Message );
				throw ex;
			}

			return orders;
		}

		public async Task< IEnumerable< Order > > GetOrdersByDateAsync( DateTime startDateUtc, DateTime endDateUtc, CancellationToken token, Mark mark )
		{
			if ( mark == null )
				mark = Mark.CreateNew();

			List< Order > orders;
			try
			{
				orders = await CollectOrdersFromAllPagesAsync( startDateUtc, endDateUtc, token, mark ).ConfigureAwait( false );
			}
			catch ( Exception ex )
			{
				LogicBrokerLogger.LogTrace( ex.Message );
				throw ex;
			}

			return orders;
		}

		private async Task< List< Order > > CollectOrdersFromAllPagesAsync( DateTime startDateUtc, DateTime endDateUtc, CancellationToken token, Mark mark )
		{
			var orders = new List< Order >();
			LogicBrokerGetOrdersResponse response;
			var paging = new Paging( base.PageSize );
			var command = new GetOrdersCommand( base.Config.DomainUrl, base.Credentials.SubscriptionKey, startDateUtc, endDateUtc, paging );
			do
			{
				response = await base.GetAsync< LogicBrokerGetOrdersResponse >( command, token, mark ).ConfigureAwait( false );
				if( response?.Records != null )
				{
					orders.AddRange( response.Records.Select( r => r.ToSvOrder() ).ToList() );
				}
				command.Paging.UpdateCurrentPage( response?.CurrentPage + 1 );
			} while( command.Paging.CurrentPage < response?.TotalPages );

			return orders;
		}

		public async Task< IEnumerable< string > > AcknowledgeOrdersAsync( IEnumerable< string > logicBrokerKeys, CancellationToken token, Mark mark )
		{
			if ( mark == null )
				mark = Mark.CreateNew();

			string acknowledgedStatus = LogicBrokerOrderStatusEnum.Acknowledged.ToString();
			var batches = logicBrokerKeys.Slice( PageSize );	//Batching is not required by LogicBroker
			var acknowledgedOrders = await batches.ProcessInBatchAsync( LogicBrokerCommand.DefaultMaxConcurrentBatches, async logicBrokerKeysBatch =>
			{
				var payload = new PutOrdersStatusPayload( acknowledgedStatus, logicBrokerKeysBatch, onlyIncreaseStatus: true );
				var command = new PutOrdersStatusCommand( base.Config.DomainUrl, base.Credentials.SubscriptionKey, payload );
				return await base.PutAsync< LogicBrokerPutOrdersStatusResponse >( command, token, mark ).ConfigureAwait( false );
			} );
			return acknowledgedOrders.SelectMany( o => o.Records.ToList() );
		}
	}
}
