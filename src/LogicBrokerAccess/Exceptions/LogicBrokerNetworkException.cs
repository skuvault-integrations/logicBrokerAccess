using System;

namespace LogicBrokerAccess.Exceptions
{
	public class LogicBrokerNetworkException : LogicBrokerException
	{
		public LogicBrokerNetworkException( string message, Exception exception ) : base( message, exception) { }
		public LogicBrokerNetworkException( string message ) : base( message ) { }
	}

	public class LogicBrokerUnauthorizedException : LogicBrokerException
	{
		public LogicBrokerUnauthorizedException( string message ) : base( message) { }
	}

	public class LogicBrokerRateLimitsExceeded : LogicBrokerNetworkException
	{
		public LogicBrokerRateLimitsExceeded( string message ) : base( message ) { }
	}
}