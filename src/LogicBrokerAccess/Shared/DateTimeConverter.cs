using System;
using System.Xml;

namespace LogicBrokerAccess.Shared
{
	public static class DateTimeConverter
	{
		public static string ToStringUtcIso8601( this DateTime dateTime )
		{
			var universalTime = dateTime.ToUniversalTime();
			var result = XmlConvert.ToString( universalTime, XmlDateTimeSerializationMode.RoundtripKind );
			return result;
		}

		//TODO GUARD-451 Documentation says dates are like this - 2015-11-13T18:34:52.411Z. This is ISO 8601
		//However, they actually return without the ending Z or the time portion. Asked LogicBroker.
		//Might need the non-Utc converter, but what about the time? Will it be filtered by time?
		//public static string ToStringIso8601( this DateTime dateTime )
		//{
		//	var result = XmlConvert.ToString( dateTime, XmlDateTimeSerializationMode.RoundtripKind );
		//	return result;
		//}

		public static DateTime ToDateTime( this string source, bool throwException = false )
		{
			DateTime dateTime;
			try
			{
				dateTime = XmlConvert.ToDateTime( source, XmlDateTimeSerializationMode.RoundtripKind | XmlDateTimeSerializationMode.Utc );
			}
			catch
			{
				dateTime = default( DateTime );
				if ( throwException )
					throw;
			}

			return dateTime;
		}
	}
}
