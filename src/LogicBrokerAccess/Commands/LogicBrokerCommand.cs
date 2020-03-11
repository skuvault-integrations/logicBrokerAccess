using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersReadyEndpointUrl = "/api/v2/Orders/Ready";

		public string Url { get { return $"{endpointUrl}{pagingUrl}"; } }
		private readonly string endpointUrl;
		public string Payload { get; protected set; }
		public const int DefaultPageSize = 100;
		protected readonly int pageSize;
		public int Page { get; private set; }
		private string pagingUrl => $"&filters.page={Page}&filters.pageSize={pageSize}";

		protected LogicBrokerCommand( string domainUrl, string endpointUrl, string subscriptionKey, string filterUrl, int pageSize = DefaultPageSize, int page = 0 )
		{
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( endpointUrl, "endpointUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			this.endpointUrl = $"{domainUrl}{endpointUrl}?subscription-key={subscriptionKey}&{filterUrl}";
			this.pageSize = pageSize;
			this.Page = page;
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
