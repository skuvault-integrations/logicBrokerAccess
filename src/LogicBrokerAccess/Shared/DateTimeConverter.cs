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
