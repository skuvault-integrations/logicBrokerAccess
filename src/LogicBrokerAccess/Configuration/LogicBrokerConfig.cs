using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Configuration
{
	public class LogicBrokerConfig
	{
		public string DomainUrl { get; }

		public readonly NetworkOptions NetworkOptions;

		public LogicBrokerConfig( string domainUrl, NetworkOptions networkOptions )
		{
			Condition.Requires( networkOptions, "networkOptions" ).IsNotNull();
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNull();

			this.NetworkOptions = networkOptions;
			this.DomainUrl = domainUrl;
		}

		public LogicBrokerConfig( string domainUrl ) : this( domainUrl, NetworkOptions.LogicBrokerDefaultNetworkOptions )
		{ }
	}

	public class NetworkOptions
	{
		public int RequestTimeoutMs { get; private set; }
		public int RetryAttempts { get; private set; }
		public int DelayBetweenFailedRequestsInSec { get; private set; }
		public int DelayFailedRequestRate { get; private set; }

		public NetworkOptions( int requestTimeoutMs, int retryAttempts, int delayBetweenFailedRequestsInSec, int delayFailedRequestRate )
		{
			Condition.Requires( requestTimeoutMs, "requestTimeoutMs" ).IsGreaterThan( 0 );
			Condition.Requires( retryAttempts, "retryAttempts" ).IsGreaterOrEqual( 0 );
			Condition.Requires( delayBetweenFailedRequestsInSec, "delayBetweenFailedRequestsInSec" ).IsGreaterOrEqual( 0 );
			Condition.Requires( delayFailedRequestRate, "delayFailedRequestRate" ).IsGreaterOrEqual( 0 );

			this.RequestTimeoutMs = requestTimeoutMs;
			this.RetryAttempts = retryAttempts;
			this.DelayBetweenFailedRequestsInSec = delayBetweenFailedRequestsInSec;
			this.DelayFailedRequestRate = delayFailedRequestRate;
		}

		public static NetworkOptions LogicBrokerDefaultNetworkOptions
		{
			get
			{
				return new NetworkOptions( 5 * 60 * 1000, 10, 5, 20 );
			}
		}
	}
}
