using CuttingEdge.Conditions;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Throttling;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersReadyEndpointUrl = "/api/v2/Orders/Ready";

		public string Url { get { return $"{endpointUrl}{Paging.PagingUrl}"; } }
		private readonly string endpointUrl;
		public string Payload { get; protected set; }
		public Paging Paging { get; private set; }
		public Throttler Throttler { get; private set; }

		protected LogicBrokerCommand( string domainUrl, string endpointUrl, string subscriptionKey, string filterUrl, ThrottlingOptions throttlingOptions, Paging paging = null )
		{
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( endpointUrl, "endpointUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();
			Condition.Requires( throttlingOptions, "throttlingOptions" ).IsNotNull();

			this.endpointUrl = $"{domainUrl}{endpointUrl}?subscription-key={subscriptionKey}&{filterUrl}";
			this.Paging = paging ?? Paging.CreateDefault();
			this.Throttler = new Throttler( throttlingOptions );
		}

		internal void UpdateCurrentPage( int? currentPage )
		{
			if( currentPage.HasValue )
			{
				this.Paging.CurrentPage = currentPage.Value;
			}
		}
	}
	
	public class Paging
	{
		public const int DefaultPageSize = 100;
		private readonly int PageSize;
		public int CurrentPage { get; set; }
		public string PagingUrl { get { return $"&filters.page={CurrentPage}&filters.pageSize={PageSize}"; } }

		public Paging( int pageSize = DefaultPageSize, int currentPage = 0 )
		{
			PageSize = pageSize;
			CurrentPage = currentPage;
		}

		public static Paging CreateDefault()
		{
			return new Paging();
		}
	}
}
