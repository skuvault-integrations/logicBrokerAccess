using LogicBrokerAccess.Shared;
using System;

namespace LogicBrokerAccess.Commands
{
	public class GetOrdersReadyCommand : LogicBrokerCommand
	{
		//TODO GUARD-451 Too many parameters
		public GetOrdersReadyCommand( string domainUrl, string subscriptionKey, DateTime startDateUtc, DateTime endDateUtc, int pageSize = DefaultPageSize, int page = 0 ) 
			: base( domainUrl, GetOrdersReadyEndpointUrl, subscriptionKey, GetOrderReadyFilterUrl( startDateUtc, endDateUtc ), pageSize, page )
		{ }

		private static string GetOrderReadyFilterUrl( DateTime startDateUtc, DateTime endDateUtc )
		{
			return $"&filters.from={startDateUtc.ToStringUtcIso8601()}&filters.to={endDateUtc.ToStringUtcIso8601()}";
		}
	}
}
