using LogicBrokerAccess.Shared;
using NUnit.Framework;
using System;
using FluentAssertions;

namespace LogicBrokerAccessTests
{
	[ TestFixture ]
	public class DateTimeConverterTests
	{
		[ Test ]
		public void ToUtcIso8601()
		{
			var year = 2030; var day = 31; var month = 3;
			var hour = 2; var minute = 51; var second = 12; var ms = 23;
			DateTime sourceDate = new DateTime( year, month, day, hour, minute, second, ms, DateTimeKind.Utc );

			var iso8601Date = sourceDate.ToStringUtcIso8601();

			iso8601Date.Should().Be( $"{year:D4}-{month:D2}-{day:D2}T{hour:D2}:{minute:D2}:{second:D2}.{ms:D3}Z" );
		}

		[ Test ]
		public void FromIso8601()
		{
			var year = 2030; var day = 31; var month = 3;
			var hour = 2; var minute = 51; var second = 12; var ms = 23;
			var utcIso8601Date = $"{year:D4}-{month:D2}-{day:D2}T{hour:D2}:{minute:D2}:{second:D2}.{ms:D3}";

			var utcDateTime = utcIso8601Date.ToDateTime();

			utcDateTime.Year.Should().Be( year );
			utcDateTime.Day.Should().Be( day );
			utcDateTime.Month.Should().Be( month );
			utcDateTime.Hour.Should().Be( hour );
			utcDateTime.Minute.Should().Be( minute );
			utcDateTime.Second.Should().Be( second );
			utcDateTime.Millisecond.Should().Be( ms );
		}
	}
}
