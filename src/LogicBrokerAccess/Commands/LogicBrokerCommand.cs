using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersReadyUrl = "/api/v2/Orders/Ready";

		public string Url { get; protected set; }
		public string Payload { get; protected set; }

		internal string GetCommandUrl( string apiBaseUrl, string relativeUrl, string subscriptionKey )
		{
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();
			Condition.Requires( relativeUrl, "relativeUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			return $"{apiBaseUrl}{relativeUrl}?subscription-key={subscriptionKey}";
		}

		//public LogicBrokerConfig Config { get; protected set; }
	}	
}
