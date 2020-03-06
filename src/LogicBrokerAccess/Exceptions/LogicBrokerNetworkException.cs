using System;

namespace LogicBrokerAccess.Exceptions
{
	public class LogicBrokerNetworkException : LogicBrokerException
	{
		public LogicBrokerNetworkException( string message, Exception exception ) : base( message, exception) { }
		public LogicBrokerNetworkException( string message ) : base( message ) { }
	}

	public class NewEggUnauthorizedException : LogicBrokerException
	{
		public NewEggUnauthorizedException( string message ) : base( message) { }
	}

	public class NewEggRateLimitsExceeded : LogicBrokerNetworkException
	{
		public NewEggRateLimitsExceeded( string message ) : base( message ) { }
	}
}