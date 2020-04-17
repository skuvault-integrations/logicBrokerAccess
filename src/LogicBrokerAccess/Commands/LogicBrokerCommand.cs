using CuttingEdge.Conditions;
using LogicBrokerAccess.Throttling;
using System.Net.Http;
using System.Text;

namespace LogicBrokerAccess.Commands
{
	public abstract class LogicBrokerCommand
	{
		protected const string GetOrdersPath = "/api/v2/Orders";
		protected const string GetOrdersReadyPath = "/api/v2/Orders/Ready";
		protected const string PutOrdersStatusPath = "/api/v2/Orders/Status";
		protected const string PostInventoryBroadcastPath = "/api/v1/Inventory/Broadcast";

		public string Url => $"{EndpointUrl}{QueryStringParams}{Paging.PagingUrl}";
		private readonly string EndpointUrl;
		protected string QueryStringParams;

		public string Payload { get; protected set; }
		public PostRequestContent PostRequestContent { get; protected set; }
		public Paging Paging { get; private set; }
		public Throttler Throttler { get; private set; }
		public const int DefaultMaxConcurrentBatches = 20;     //Hard "burst" limit is 25		

		protected LogicBrokerCommand( string domainUrl, string commandUrl, string subscriptionKey, ThrottlingOptions throttlingOptions, Paging paging = null )
		{
			this.EndpointUrl = new BaseCommandUrl( domainUrl, commandUrl, subscriptionKey ).Url;
			this.Paging = paging ?? Paging.CreateDisabled();
			this.Throttler = new Throttler( throttlingOptions ?? ThrottlingOptions.LogicBrokerDefaultThrottlingOptions );
		}
	}

	public class BaseCommandUrl
	{
		public readonly string Url;

		public BaseCommandUrl( string domainUrl, string endpointPath, string subscriptionKey )
		{
			Condition.Requires( domainUrl, "domainUrl" ).IsNotNullOrWhiteSpace();
			Condition.Requires( endpointPath, "endpointPath" ).IsNotNullOrWhiteSpace();
			Condition.Requires( subscriptionKey, "subscriptionKey" ).IsNotNullOrWhiteSpace();

			this.Url = $"{domainUrl}{endpointPath}?subscription-key={subscriptionKey}";
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

	public class PostRequestContent
	{
		private const string FileFieldKey = "file";
		private const string PostBoundary = "----SKUVAULT_BOUNDARY";

		public HttpContent Content { get; private set; }

		public PostRequestContent( string postFileContents )
		{
			var content = new MultipartFormDataContent( PostBoundary );
			content.Headers.ContentType.MediaType = "multipart/form-data";
			content.Add( new StringContent( postFileContents, Encoding.UTF8 ), FileFieldKey );
			this.Content = content;
		}
	}
}
