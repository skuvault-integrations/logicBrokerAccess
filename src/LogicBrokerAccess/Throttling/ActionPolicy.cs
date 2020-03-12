using CuttingEdge.Conditions;
using LogicBrokerAccess.Exceptions;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using LogicBrokerAccess.Commands;
using LogicBrokerAccess.Configuration;

namespace LogicBrokerAccess.Throttling
{
	public class ActionPolicy
	{
		private readonly int _retryAttempts;
		private readonly int _delay;
		private readonly int _delayRate;

		public ActionPolicy( NetworkOptions networkOptions )
		{
			Condition.Requires( networkOptions, "networkOptions" ).IsNotNull();
			Condition.Requires( networkOptions.RetryAttempts, "networkOptions.RetryAttempts" ).IsGreaterThan( 0 );
			Condition.Requires( networkOptions.DelayBetweenFailedRequestsInSec, "networkOptions.DelayBetweenFailedRequestsInSec" ).IsGreaterOrEqual( 0 );
			Condition.Requires( networkOptions.DelayFailRequestRate, "networkOptions.DelayFailRequestRate" ).IsGreaterOrEqual( 0 );

			this._retryAttempts = networkOptions.RetryAttempts;
			this._delay = networkOptions.DelayBetweenFailedRequestsInSec;
			this._delayRate = networkOptions.DelayFailRequestRate;
		}

		/// <summary>
		///	Retries function until it succeed or failed
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="funcToThrottle"></param>
		/// <param name="onRetryAttempt">Retry attempts</param>
		/// <param name="extraLogInfo"></param>
		/// <param name="onException"></param>
		/// <returns></returns>
		public Task< TResult > ExecuteAsync< TResult >( Func< Task< TResult > > funcToThrottle, Action< Exception, TimeSpan, int > onRetryAttempt, Func< string > extraLogInfo, Action< Exception > onException )
		{
			return Policy.Handle< LogicBrokerNetworkException >()
				.WaitAndRetryAsync( _retryAttempts,
					retryCount => TimeSpan.FromSeconds( this.GetDelayBeforeNextAttempt( retryCount ) ),
					( exception, timeSpan, retryCount, context ) =>
					{
						onRetryAttempt?.Invoke( exception, timeSpan, retryCount );
					})
				.ExecuteAsync( async () =>
				{
					try
					{
						return await funcToThrottle().ConfigureAwait( false );
					}
					catch ( Exception exception )
					{
						if ( exception is LogicBrokerNetworkException )
							throw exception;

						LogicBrokerException LogicBrokerException = null;

						var exceptionDetails = string.Empty;

						if ( extraLogInfo != null )
							exceptionDetails = extraLogInfo();

						if ( exception is HttpRequestException )
							LogicBrokerException = new LogicBrokerNetworkException( exceptionDetails, exception );
						else
						{
							LogicBrokerException = new LogicBrokerException( exceptionDetails, exception );
							onException?.Invoke( LogicBrokerException );
						}

						throw LogicBrokerException;
					}
				});
		}

		public int GetDelayBeforeNextAttempt( int retryCount )
		{
			return this._delay + this._delayRate * retryCount;
		}
	}
}