using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace LogicBrokerAccess.Shared
{
	public static class JsonSerializer
	{
		public static string ToJson( this object source )
		{
			try
			{
				if ( source == null )
					return "{}";
				else
				{
					var serialized = JsonConvert.SerializeObject( source, new IsoDateTimeConverter() );
					return serialized;
				}
			}
			catch ( Exception )
			{
				return "{}";
			}
		}
	}
}
