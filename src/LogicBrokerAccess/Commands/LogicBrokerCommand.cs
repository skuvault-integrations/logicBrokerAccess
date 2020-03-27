using CuttingEdge.Conditions;
using LogicBrokerAccess.Throttling;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersPath = "/api/v2/Orders";
		protected const string GetOrdersReadyPath = "/api/v2/Orders/Ready";
		protected const string PutOrdersStatusPath = "/api/v2/Orders/Status";

		public string Url => $"{EndpointUrl}{Paging.PagingUrl}";
		protected string EndpointUrl;
		public string Payload { get; protected set; }
		public Paging Paging { get; private set; }
		public Throttler Throttler { get; private set; }
		public const int DefaultMaxConcurrentBatches = 20;     //Hard "burst" limit is 25

		protected LogicBrokerCommand( string commandUrl, ThrottlingOptions throttlingOptions, Paging paging = null )
		{
			this.EndpointUrl = commandUrl;
			this.Paging = paging ?? Paging.CreateDisabled();
			this.Throttler = new Throttler( throttlingOptions ?? ThrottlingOptions.LogicBrokerDefaultThrottlingOptions );
		}
	}

	public class BaseCommandUrl
	{
		public readonly string Url;

		public BaseCommandUrl( string domainUrl, string endpointUrl, string subscriptionKey )
		{
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( endpointUrl, "endpointUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			this.Url = $"{domainUrl}{endpointUrl}?subscription-key={subscriptionKey}";
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
		public string PagingUrl => isEnabled ? $"&filters.page={CurrentPage}&filters.pageSize={PageSize}" : "";
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
