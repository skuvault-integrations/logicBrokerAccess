using CuttingEdge.Conditions;
using LogicBrokerAccess.Throttling;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersPath = "/api/v2/Orders";
		protected const string GetOrdersReadyPath = "/api/v2/Orders/Ready";
		protected const string PutOrdersStatusPath = "/api/v2/Orders/Status";

		public string Url { get { return $"{endpointUrl}{Paging.PagingUrl}"; } }
		private readonly string endpointUrl;
		public string PayloadJson { get; protected set; }
		public Paging Paging { get; private set; }
		public Throttler Throttler { get; private set; }
		public const int MaxConcurrentBatches = 20;     //Hard "burst" limit is 25

		protected LogicBrokerCommand( BaseCommandUrl commandUrl, Payload payload, ThrottlingOptions throttlingOptions )
			: this( commandUrl, throttlingOptions, Paging.CreateDisabled() ) 
		{
			this.PayloadJson = payload.JsonObject;
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
		public string PagingUrl { get 
		{ 
			return isEnabled ? $"&filters.page={CurrentPage}&filters.pageSize={PageSize}" : ""; 
		} }
		public bool isEnabled = true;

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

		public static Paging CreateDisabled()
		{
			var paging = new Paging
			{
				isEnabled = false
			};
			return paging;
		}
	}
}
