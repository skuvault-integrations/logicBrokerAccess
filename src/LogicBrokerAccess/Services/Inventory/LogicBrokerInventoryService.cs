using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Models;
using Netco.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogicBrokerAccess.Services.Inventory
{
	public class LogicBrokerInventoryService : BaseService, ILogicBrokerInventoryService
	{
		public LogicBrokerInventoryService( LogicBrokerConfig config, LogicBrokerCredentials credentials )
			: base( config, credentials, Paging.DefaultPageSize )
		{ }

		public async Task< string > UpdateInventoryAsync( IEnumerable< LogicBrokerInventoryItem > inventoryItems, CancellationToken token, Mark mark )
		{
			if( mark == null )
				mark = Mark.CreateNew();

			var command = new PostInventoryBroadcastCommand( base.Config.DomainUrl, base.Credentials.SubscriptionKey, inventoryItems );
			var result = await base.PostAsync< LogicBrokerPostInventoryBroadcastResponse >( command, token, mark ).ConfigureAwait( false );
			return result.Body;
		}
	}
}
