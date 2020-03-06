using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Configuration
{
	public class LogicBrokerCredentials
	{
		public string SubscriptionKey { get; private set; }

		public LogicBrokerCredentials( string subscriptionKey )
		{
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			this.SubscriptionKey = subscriptionKey;
		}
	}
}
