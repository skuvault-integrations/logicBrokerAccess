using CuttingEdge.Conditions;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Throttling;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersEndpointUrl = "/api/v2/Orders";
		protected const string GetOrdersReadyEndpointUrl = "/api/v2/Orders/Ready";

		public string Url { get { return $"{endpointUrl}{Paging.PagingUrl}"; } }
		private readonly string endpointUrl;
		public string Payload { get; protected set; }
		public Paging Paging { get; private set; }
		public Throttler Throttler { get; private set; }

		protected LogicBrokerCommand( BaseCommandUrl commandUrl, Payload payload, ThrottlingOptions throttlingOptions, Paging paging )
			: this( commandUrl, throttlingOptions, paging ) 
		{
			this.Payload = payload.JsonObject;
		}

		protected LogicBrokerCommand( BaseCommandUrl commandUrl, string filterUrl, ThrottlingOptions throttlingOptions, Paging paging )
			: this( commandUrl, throttlingOptions, paging )
		{
			this.endpointUrl += filterUrl;
		}

		private LogicBrokerCommand( BaseCommandUrl commandUrl, ThrottlingOptions throttlingOptions, Paging paging )
		{
			this.endpointUrl = commandUrl.url;
			this.Paging = paging ?? Paging.CreateDefault();
			this.Throttler = new Throttler( throttlingOptions ?? ThrottlingOptions.LogicBrokerDefaultThrottlingOptions );
		}
	}

	public class BaseCommandUrl
	{
		public readonly string url;
		public readonly string subscriptionKey;
		public readonly string domainUrl;

		public BaseCommandUrl( string domainUrl, string endpointUrl, string subscriptionKey )
		{
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( endpointUrl, "endpointUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			this.url = $"{domainUrl}{endpointUrl}?subscription-key={subscriptionKey}";
		}
	}

	public class Payload
	{
		public string JsonObject { get; private set; }
		public Payload( string payloadJson )
		{
			JsonObject = payloadJson;
		}
	}

	public class Paging
	{
		public const int DefaultPageSize = 100;
		private readonly int PageSize;
		public int CurrentPage { get; private set; }
		public string PagingUrl { get { return $"&filters.page={CurrentPage}&filters.pageSize={PageSize}"; } }

		public Paging( int pageSize = DefaultPageSize, int currentPage = 0 )
		{
			PageSize = pageSize;
			CurrentPage = currentPage;
		}

		public void UpdateCurrentPage( int? currentPage )
		{
			if ( currentPage.HasValue )
			{
				this.CurrentPage = currentPage.Value;
			}
		}

		public static Paging CreateDefault()
		{
			return new Paging();
		}
	}
}
