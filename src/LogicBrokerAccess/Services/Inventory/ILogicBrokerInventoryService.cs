using LogicBrokerAccess.Models;
using Netco.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogicBrokerAccess.Services.Inventory
{
	public interface ILogicBrokerInventoryService
	{
		Task< string > UpdateInventoryAsync( IEnumerable< LogicBrokerInventoryItem > inventoryItems, CancellationToken token, Mark mark );
	}
}
