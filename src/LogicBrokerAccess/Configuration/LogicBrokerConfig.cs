using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Configuration
{
	public class LogicBrokerConfig
	{
		public string DomainUrl { get; }

		public readonly int ThrottlingMaxRetryAttempts;
		public readonly NetworkOptions NetworkOptions;

		public LogicBrokerConfig( string domainUrl, int throttlingMaxRetryAttempts, NetworkOptions networkOptions )
		{
			Condition.Requires( throttlingMaxRetryAttempts, "throttlingMaxRetryAttempts" ).IsGreaterThan( 0 );
			Condition.Requires( networkOptions, "networkOptions" ).IsNotNull();
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNull();

			this.ThrottlingMaxRetryAttempts = throttlingMaxRetryAttempts;
			this.NetworkOptions = networkOptions;
			this.DomainUrl = domainUrl;
		}

		public LogicBrokerConfig( string domainUrl ) : this( domainUrl, ThrottlingOptions.LogicBrokerDefaultThrottlingOptions.MaxRetryAttempts, NetworkOptions.LogicBrokerDefaultNetworkOptions )
		{ }
	}

	public class ThrottlingOptions
	{
		public int MaxRequestsPerTimeInterval { get; private set; }
		public int TimeIntervalInSec { get; private set; }
		public int MaxRetryAttempts { get; private set; }

		public ThrottlingOptions( int maxRequests, int timeIntervalInSec, int maxRetryAttempts )
		{
			Condition.Requires( maxRequests, "maxRequests" ).IsGreaterOrEqual( 1 );
			Condition.Requires( timeIntervalInSec, "timeIntervalInSec" ).IsGreaterOrEqual( 1 );
			Condition.Requires( maxRetryAttempts, "maxRetryAttempts" ).IsGreaterOrEqual( 0 );

			this.MaxRequestsPerTimeInterval = maxRequests;
			this.TimeIntervalInSec = timeIntervalInSec;
			this.MaxRetryAttempts = maxRetryAttempts;
		}

		public static ThrottlingOptions LogicBrokerDefaultThrottlingOptions
		{
			get
			{
				return new ThrottlingOptions( 4, 1, 10 );
			}
		}
	}

	public class NetworkOptions
	{
		public int RequestTimeoutMs { get; private set; }
		public int RetryAttempts { get; private set; }
		public int DelayBetweenFailedRequestsInSec { get; private set; }
		public int DelayFailRequestRate { get; private set; }

		public NetworkOptions( int requestTimeoutMs, int retryAttempts, int delayBetweenFailedRequestsInSec, int delayFaileRequestRate )
		{
			Condition.Requires( requestTimeoutMs, "requestTimeoutMs" ).IsGreaterThan( 0 );
			Condition.Requires( retryAttempts, "retryAttempts" ).IsGreaterOrEqual( 0 );
			Condition.Requires( delayBetweenFailedRequestsInSec, "delayBetweenFailedRequestsInSec" ).IsGreaterOrEqual( 0 );
			Condition.Requires( delayFaileRequestRate, "delayFaileRequestRate" ).IsGreaterOrEqual( 0 );

			this.RequestTimeoutMs = requestTimeoutMs;
			this.RetryAttempts = retryAttempts;
			this.DelayBetweenFailedRequestsInSec = delayBetweenFailedRequestsInSec;
			this.DelayFailRequestRate = delayFaileRequestRate;
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
