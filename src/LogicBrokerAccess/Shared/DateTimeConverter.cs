using System;
using System.Xml;

namespace LogicBrokerAccess.Shared
{
	public static class DateTimeConverter
	{
		public static string ToIso8601DateString( this DateTime dateTimeUtc )
		{
			var result = XmlConvert.ToString( dateTimeUtc, XmlDateTimeSerializationMode.RoundtripKind );
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
