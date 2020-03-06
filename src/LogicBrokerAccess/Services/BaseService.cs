using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;
using LogicBrokerAccess.Exceptions;
using LogicBrokerAccess.Throttling;
using LogicBrokerAccess.Shared;
using Netco.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CuttingEdge.Conditions;

namespace LogicBrokerAccess.Services
{
	public class BaseService
	{
		protected LogicBrokerCredentials Credentials { get; private set; }
		protected LogicBrokerConfig Config { get; private set; }
		protected Throttler Throttler { get; private set; }
		protected HttpClient HttpClient { get; private set; }
		protected Func< string > _additionalLogInfo;

		public Func< string > AdditionalLogInfo
		{
			get { return this._additionalLogInfo ?? ( () => string.Empty ); }
			set => _additionalLogInfo = value;
		}

		public BaseService( LogicBrokerCredentials credentials, LogicBrokerConfig config )
		{
			Condition.Requires( credentials, "credentials" ).IsNotNull();
			Condition.Requires( config, "config" ).IsNotNull();

			this.Credentials = credentials;
			this.Config = config;
			this.Throttler = new Throttler( config.ThrottlingOptions.MaxRequestsPerTimeInterval, config.ThrottlingOptions.TimeIntervalInSec, config.ThrottlingOptions.MaxRetryAttempts );
			HttpClient = new HttpClient();
		}

		protected async Task< T > GetAsync< T >( LogicBrokerCommand command, CancellationToken cancellationToken, Mark mark )
		{
			if ( mark == null )
				mark = Mark.CreateNew();

			if ( cancellationToken.IsCancellationRequested )
			{
				var exceptionDetails = this.CreateMethodCallInfo( command.Url, mark, additionalInfo: this.AdditionalLogInfo() );
				throw new LogicBrokerException( string.Format( "{0}. Task was cancelled", exceptionDetails ) );
			}

			var responseContent = await this.ThrottleRequestAsync( command, mark, async ( token ) =>
			{
				var httpResponse = await HttpClient.GetAsync( command.Url ).ConfigureAwait( false );
				var content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait( false );

				ThrowIfError( httpResponse, content );

				return content;
			}, cancellationToken ).ConfigureAwait( false );

			var response = JsonConvert.DeserializeObject< T >( responseContent );

			return response;
		}

		//TODO Error responses (invalid subscription-key)
		//{
		//    "Code": "Unauthorized",
		//    "Message": "Unauthorized Access. Invalid subscription key.",
		//    "Body": null
		//}

		private void ThrowIfError( HttpResponseMessage response, string message )
		{
			var responseStatusCode = response.StatusCode;

			if ( response.IsSuccessStatusCode )
				return;

			if ( responseStatusCode == HttpStatusCode.Unauthorized )
			{
				throw new LogicBrokerException( message );
			}

			throw new LogicBrokerNetworkException( message );
		}

		private Task< T > ThrottleRequestAsync< T >( LogicBrokerCommand command, Mark mark, Func< CancellationToken, Task< T > > processor, CancellationToken token )
		{
			return Throttler.ExecuteAsync( () =>
			{
				return new ActionPolicy( Config.NetworkOptions.RetryAttempts, Config.NetworkOptions.DelayBetweenFailedRequestsInSec, Config.NetworkOptions.DelayFailRequestRate )
					.ExecuteAsync( async () =>
					{
						//Misc.InitSecurityProtocol();

						using( var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource( token ) )
						{
							LogicBrokerLogger.LogStarted( this.CreateMethodCallInfo( command.Url, mark, payload: command.Payload, additionalInfo: this.AdditionalLogInfo() ) );
							linkedTokenSource.CancelAfter( Config.NetworkOptions.RequestTimeoutMs );

							var result = await processor( linkedTokenSource.Token ).ConfigureAwait( false );

							LogicBrokerLogger.LogEnd( this.CreateMethodCallInfo( command.Url, mark, methodResult: result.ToJson(), additionalInfo: this.AdditionalLogInfo() ) );

							return result;
						}
					}, 
					( exception, timeSpan, retryCount ) =>
					{
						string retryDetails = this.CreateMethodCallInfo( command.Url, mark, additionalInfo: this.AdditionalLogInfo() );
						LogicBrokerLogger.LogTraceRetryStarted( timeSpan.Seconds, retryCount, retryDetails );
					},
					() => CreateMethodCallInfo( command.Url, mark, additionalInfo: this.AdditionalLogInfo() ),
					LogicBrokerLogger.LogTraceException );
			} );
		}

		private string CreateMethodCallInfo( string url = "", Mark mark = null, string errors = "", string methodResult = "", string additionalInfo = "", string payload = "", [ CallerMemberName ] string memberName = "" )
		{
			string serviceEndPoint = null;
			string requestParameters = null;

			if ( !string.IsNullOrEmpty( url ) )
			{
				Uri uri = new Uri( url );

				serviceEndPoint = uri.LocalPath;
				requestParameters = uri.Query;
			}

			var str = string.Format(
				"{{MethodName: {0}, Mark: '{1}', ServiceEndPoint: '{2}', {3} {4}{5}{6}{7}}}",
				memberName,
				mark ?? Mark.Blank(),
				string.IsNullOrWhiteSpace( serviceEndPoint ) ? string.Empty : serviceEndPoint,
				string.IsNullOrWhiteSpace( requestParameters ) ? string.Empty : ", RequestParameters: " + requestParameters,
				string.IsNullOrWhiteSpace( errors ) ? string.Empty : ", Errors:" + errors,
				string.IsNullOrWhiteSpace( methodResult ) ? string.Empty : ", Result:" + methodResult,
				string.IsNullOrWhiteSpace( additionalInfo ) ? string.Empty : ", " + additionalInfo,
				string.IsNullOrWhiteSpace( payload ) ? string.Empty : ", " + payload
			);
			return str;
		}
	}
}
