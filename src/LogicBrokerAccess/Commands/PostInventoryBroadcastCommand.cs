using CsvHelper;
using LogicBrokerAccess.Models;
using LogicBrokerAccess.Throttling;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LogicBrokerAccess.Commands
{
	public class PostInventoryBroadcastCommand : LogicBrokerCommand
	{
		//Request rate limited to 1 request every 60 seconds with bursts up to 2 requests.
		private const int MaxRequestsPerTimeInterval = 1;
		private const int TimeIntervalInSec = 90;

		public PostInventoryBroadcastCommand( string domainUrl, string subscriptionKey, IEnumerable< LogicBrokerInventoryItem > inventoryItems ) 
			: base( domainUrl, PostInventoryBroadcastPath, subscriptionKey, GetThrottlingOptions() )
		{
			string inventoryUpdateCsvText = CreateInventoryUpdateCsv( inventoryItems );
			base.Payload = inventoryUpdateCsvText;	//Populate for logging
			base.PostRequestContent = new PostRequestContent( base.Payload );
			base.QueryStringParams = GetQueryStringParams();
		}

		private static string CreateInventoryUpdateCsv( IEnumerable< LogicBrokerInventoryItem > inventoryItems )
		{
			var sb = new StringBuilder();
			using ( var writer = new StringWriter( sb ) )
			using ( var csvWriter = new CsvWriter( writer, CultureInfo.InvariantCulture ) )
			{
				csvWriter.WriteHeader< LogicBrokerInventoryItem >();
				csvWriter.NextRecord();
				csvWriter.WriteRecords( inventoryItems );
			}
			return sb.ToString();
		}

		private static string GetQueryStringParams()
		{
			return "&transform=false";
		}

		private static ThrottlingOptions GetThrottlingOptions()
		{
			return new ThrottlingOptions( MaxRequestsPerTimeInterval, TimeIntervalInSec );
		}
	}

	public class LogicBrokerPostInventoryBroadcastResponse
	{
		public string Code { get; set; }
		public string Message { get; set; }
		public string Body { get; set; }
	}
}
