using Newtonsoft.Json;

namespace LogicBrokerAccess.Models
{
	public class LogicBrokerOrderRecords
	{
		[ JsonProperty( "Records" ) ]
		public LogicBrokerOrder[] Records { get; set; }
	}

	public class LogicBrokerOrder
	{
		[ JsonProperty( "OrderNumber" ) ]
		public string OrderNumber { get; set; }

		//TODO GUARD-451 Add more fields
	}

	public static class OrderExtensions
	{
		public static Order ToSvOrder( this LogicBrokerOrder logicBrokerOrder )
		{
			return new Order
			{
				OrderNumber = logicBrokerOrder.OrderNumber
			};
		}
		//TODO GUARD-451 Add any needed converters
	}

}
