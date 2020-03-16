using LogicBrokerAccess.Shared;
using LogicBrokerAccess.Throttling;
using System;

namespace LogicBrokerAccess.Commands
{
	public class GetOrdersCommand : LogicBrokerCommand
	{
		//Request rate limited to 1 request every 2 seconds with bursts up to 25 requests.
		private const int MaxRequestsPerTimeInterval = 1;
		private const int TimeIntervalInSec = 3;

		public GetOrdersCommand( string domainUrl, string subscriptionKey, DateTime startDateUtc, DateTime endDateUtc, Paging paging = null ) 
			: base( GetCommandUrl( domainUrl, subscriptionKey ), GetOrderFilterUrl( startDateUtc, endDateUtc ), GetThrottlingOptions(), paging )
		{ }

		private static BaseCommandUrl GetCommandUrl( string domainUrl, string subscriptionKey )
		{
			return new BaseCommandUrl( domainUrl, GetOrdersPath, subscriptionKey );
		}

		private static string GetOrderFilterUrl( DateTime startDateUtc, DateTime endDateUtc )
		{
			return $"&filters.from={startDateUtc.ToStringUtcIso8601()}&filters.to={endDateUtc.ToStringUtcIso8601()}";
		}

		private static ThrottlingOptions GetThrottlingOptions()
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec);
		}
	}
}
