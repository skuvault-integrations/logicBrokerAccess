using CuttingEdge.Conditions;
using LogicBrokerAccess.Shared;
using LogicBrokerAccess.Throttling;
using System.Collections.Generic;

namespace LogicBrokerAccess.Commands
{
	public class PutOrdersStatusCommand : LogicBrokerCommand
	{
		//Request rate limited to 1 request per second with bursts up to 25 requests.
		private const int MaxRequestsPerTimeInterval = 1;
		private const int TimeIntervalInSec = 2;

		public PutOrdersStatusCommand( string domainUrl, string subscriptionKey, PutOrdersStatusPayload payload )
			: base( domainUrl, PutOrdersStatusPath, subscriptionKey , GetThrottlingOptions() )
		{ 
			this.Payload = payload.ToJson();
		}

		private static ThrottlingOptions GetThrottlingOptions()
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec );
		}
	}

	public class PutOrdersStatusPayload
	{
		public string Status { get; }   //Text, not code:(
		public bool OnlyIncreaseStatus { get; }
		public IEnumerable< string > LogicbrokerKeys { get; }

		public PutOrdersStatusPayload( string status, IEnumerable< string > logicbrokerKeys, bool onlyIncreaseStatus = true )
		{
			Condition.Requires( status, "status" ).IsNotNullOrWhiteSpace();
			Condition.Requires( logicbrokerKeys, "logicbrokerKeys" ).IsNotNull().IsNotEmpty();

			this.Status = status;
			this.LogicbrokerKeys = logicbrokerKeys;
			this.OnlyIncreaseStatus = onlyIncreaseStatus;
		}
	}

	public class LogicBrokerPutOrdersStatusResponse
	{
		public string [] Records { get; set; }
		public int TotalRecords { get; set; }
	}
}
