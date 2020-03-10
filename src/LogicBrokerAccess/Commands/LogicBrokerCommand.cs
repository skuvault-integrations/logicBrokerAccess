using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersReadyEndpointUrl = "/api/v2/Orders/Ready";

		public string Url { get { return $"{EndpointUrl}{PagingUrl}"; } }
		public string EndpointUrl { get; protected set; }
		public string Payload { get; protected set; }
		public const int DefaultPageSize = 100;
		private string PagingUrl => $"&filters.page={Page}&filters.pageSize={PageSize}";
		public int Page { get; private set; }
		public int PageSize { get; }

		protected LogicBrokerCommand( string domainUrl, string endpointUrl, string filterUrl, string subscriptionKey, int pageSize = DefaultPageSize, int page = 0 )
		{
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( endpointUrl, "relativeUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			EndpointUrl = $"{domainUrl}{endpointUrl}?subscription-key={subscriptionKey}&{filterUrl}";
			PageSize = pageSize;
			Page = page;
		}

		internal void UpdateCurrentPage( int? currentPage )
		{
			if( currentPage.HasValue )
			{
				this.Page = currentPage.Value;
			}
		}
	}	
}
