using LogicBrokerAccess.Models;
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
			: base( domainUrl, GetOrdersPath, subscriptionKey, GetThrottlingOptions(), paging )
		{ 
			this.QueryStringParams = GetOrderFilterUrl( startDateUtc, endDateUtc );
		}

		private static string GetOrderFilterUrl( DateTime startDateUtc, DateTime endDateUtc )
		{
			return $"&filters.from={startDateUtc.ToIso8601DateString()}&filters.to={endDateUtc.ToIso8601DateString()}";
		}

		private static ThrottlingOptions GetThrottlingOptions()
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec);
		}
	}

	public class LogicBrokerGetOrdersResponse
	{
		public LogicBrokerOrder [] Records { get; set; }
		public int TotalPages { get; set; }
		public int CurrentPage { get; set; }
	}
}
