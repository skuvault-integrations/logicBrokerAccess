using CsvHelper;
using CsvHelper.Configuration;
using LogicBrokerAccess.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LogicBrokerAccessTests
{
	public abstract class BaseTest
	{
		public LogicBrokerCredentials Credentials { get; }

		public BaseTest()
		{
			var testCredentials = this.LoadTestSettings< TestCredentials >( @"\..\..\credentials.csv" );
			this.Credentials = new LogicBrokerCredentials( testCredentials.SubscriptionKey );
		}

		protected T LoadTestSettings< T >( string filePath )
		{
			string basePath = new Uri( Path.GetDirectoryName( Assembly.GetExecutingAssembly().CodeBase ) ).LocalPath;

			using ( var streamReader = new StreamReader( basePath + filePath ) )
			{
				var csvConfig = new Configuration()
				{
					Delimiter = ","
				};

				using ( var csvReader = new CsvReader( streamReader, csvConfig ) )
				{
					var credentials = csvReader.GetRecords< T >();

					return credentials.FirstOrDefault();
				}
			}
		}
	}
}
