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
			: base( GetCommandUrl( domainUrl, subscriptionKey ), new Payload( payload.ToJson() ), GetThrottlingOptions() )
		{ }

		private static string GetCommandUrl( string domainUrl, string subscriptionKey )
		{
			return new BaseCommandUrl( domainUrl, PutOrdersStatusPath, subscriptionKey ).Url;
		}

		private static ThrottlingOptions GetThrottlingOptions()
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec );
		}
	}

	public class PutOrdersStatusPayload
	{
		public string Status;   //Text, not code:(
		public bool OnlyIncreaseStatus;
		public IEnumerable< string > LogicbrokerKeys;

		public PutOrdersStatusPayload( string status, IEnumerable< string > logicbrokerKeys, bool onlyIncreaseStatus = true )
		{
			Condition.Requires( status, "status" ).IsNotNullOrWhiteSpace();
			Condition.Requires( logicbrokerKeys, "logicbrokerKeys" ).IsNotNull().IsNotEmpty();

			this.Status = status;
			this.LogicbrokerKeys = logicbrokerKeys;
			this.OnlyIncreaseStatus = onlyIncreaseStatus;
		}
	}
}
