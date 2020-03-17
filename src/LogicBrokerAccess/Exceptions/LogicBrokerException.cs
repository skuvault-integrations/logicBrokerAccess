using System;

namespace LogicBrokerAccess.Exceptions
{
	public class LogicBrokerException : Exception
	{
		public LogicBrokerException( string message, Exception exception ) : base( message, exception ) { }
		public LogicBrokerException( string message ) : this( message, null ) { }
	}
}
