﻿using CsvHelper;
using CsvHelper.Configuration;
using LogicBrokerAccess.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LogicBrokerAccessTests
{
	public abstract class BaseTest
	{
		private readonly string sandboxDomainUri = "https://stage.commerceapi.io";

		public LogicBrokerCredentials Credentials { get; }
		public LogicBrokerConfig Config { get; }

		public BaseTest()
		{
			var testCredentials = this.LoadTestSettings< TestCredentials >( @"\..\..\Files\credentials.csv" );
			this.Credentials = new LogicBrokerCredentials( testCredentials.SubscriptionKey );
			this.Config = new LogicBrokerConfig( sandboxDomainUri );
		}

		protected T LoadTestSettings< T >( string filePath )
		{
			string basePath = new Uri( Path.GetDirectoryName( Assembly.GetExecutingAssembly().CodeBase ) ).LocalPath;

			using ( var streamReader = new StreamReader( basePath + filePath ) )
			{
				var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
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
