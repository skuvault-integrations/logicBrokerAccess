using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Configuration
{
	public class LogicBrokerConfig
	{
		public string ApiBaseUrl { get; }

		public readonly ThrottlingOptions ThrottlingOptions;
		public readonly NetworkOptions NetworkOptions;

		public LogicBrokerConfig( string apiBaseUrl, ThrottlingOptions throttlingOptions, NetworkOptions networkOptions )
		{
			Condition.Requires( throttlingOptions, "throttlingOptions" ).IsNotNull();
			Condition.Requires( networkOptions, "networkOptions" ).IsNotNull();
			Condition.Requires( apiBaseUrl, "apiBaseUrl" ).IsNotNull();

			this.ThrottlingOptions = throttlingOptions;
			this.NetworkOptions = networkOptions;
			this.ApiBaseUrl = apiBaseUrl;
		}

		public LogicBrokerConfig( string apiBaseUrl ) : this( apiBaseUrl, ThrottlingOptions.LogicBrokerDefaultThrottlingOptions, NetworkOptions.LogicBrokerDefaultNetworkOptions )
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
