namespace LogicBrokerAccess.Commands
{
	public class GetOrdersReadyCommand : LogicBrokerCommand
	{
		public GetOrdersReadyCommand( string apiBaseUrl, string subscriptionKey )
		{
			this.Url = base.GetCommandUrl( apiBaseUrl, GetOrdersReadyUrl, subscriptionKey );
		}
	}
}
