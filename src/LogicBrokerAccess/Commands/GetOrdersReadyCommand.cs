using LogicBrokerAccess.Shared;
using System;

namespace LogicBrokerAccess.Commands
{
	public class GetOrdersReadyCommand : LogicBrokerCommand
	{
		public GetOrdersReadyCommand( string apiBaseUrl, string subscriptionKey, DateTime startDateUtc, DateTime endDateUtc )
		{
			var baseRequestUrl = base.GetCommandUrl( apiBaseUrl, GetOrdersReadyUrl, subscriptionKey );
			this.Url = $"{baseRequestUrl}&filters.from={startDateUtc.ToStringUtcIso8601()}&filters.to={endDateUtc.ToStringUtcIso8601()}";
		}
	}
}
