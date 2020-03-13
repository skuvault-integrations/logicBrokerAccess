using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Shared;
using System;

namespace LogicBrokerAccess.Commands
{
	public class GetOrdersCommand : LogicBrokerCommand
	{
		//Request rate limited to 1 request every 2 seconds
		private const int MaxRequestsPerTimeInterval = 1;
		private const int TimeIntervalInSec = 3;

		public GetOrdersCommand( string domainUrl, string subscriptionKey, DateTime startDateUtc, DateTime endDateUtc, int throttlingMaxRetryAttempts, Paging paging = null ) 
			: base( domainUrl, GetOrdersEndpointUrl, subscriptionKey, GetOrderFilterUrl( startDateUtc, endDateUtc ), GetThrottlingOptions( throttlingMaxRetryAttempts ), paging )
		{ }

		private static string GetOrderFilterUrl( DateTime startDateUtc, DateTime endDateUtc )
		{
			return $"&filters.from={startDateUtc.ToStringUtcIso8601()}&filters.to={endDateUtc.ToStringUtcIso8601()}";
		}

		private static ThrottlingOptions GetThrottlingOptions( int throttlingMaxRetryAttempts )
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec, throttlingMaxRetryAttempts );
		}
	}
}
