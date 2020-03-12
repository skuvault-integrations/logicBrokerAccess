using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Shared;
using System;

namespace LogicBrokerAccess.Commands
{
	public class GetOrdersReadyCommand : LogicBrokerCommand
	{
		//Request rate limited to 1 request every 2 seconds
		private const int MaxRequestsPerTimeInterval = 1;
		private const int TimeIntervalInSec = 3;

		public GetOrdersReadyCommand( string domainUrl, string subscriptionKey, DateTime startDateUtc, DateTime endDateUtc, int throttlingMaxRetryAttempts, Paging paging = null ) 
			: base( domainUrl, GetOrdersReadyEndpointUrl, subscriptionKey, GetOrderReadyFilterUrl( startDateUtc, endDateUtc ), GetThrottlingOptions( throttlingMaxRetryAttempts ), paging )
		{ }

		private static string GetOrderReadyFilterUrl( DateTime startDateUtc, DateTime endDateUtc )
		{
			return $"&filters.from={startDateUtc.ToStringUtcIso8601()}&filters.to={endDateUtc.ToStringUtcIso8601()}";
		}

		private static ThrottlingOptions GetThrottlingOptions( int throttlingMaxRetryAttempts )
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec, throttlingMaxRetryAttempts );
		}
	}
}
