using System;

namespace LogicBrokerAccess.Commands
{
	public class LogicBrokerException : Exception
	{
		public LogicBrokerException( string message, Exception exception ) : base( message, exception ) { }
		public LogicBrokerException( string message ) : this( message, null ) { }
	}
}
