using CSharpFunctionalExtensions;
using MediatR;
using MediatR.Pipeline;
using Serilog;

namespace AirportSystem.Application.Behaviour
{
    public class RequestExceptionBehaviour<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException> where TRequest : IRequest<TResponse> where TException : Exception
    {
        public async Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
        {

            Result.Failure("UNHANDLED EXCEPTION OCCURED --- CHECK LOGS.");
            state.ToResult(exception.Message);
            Log.Error($"Exception occurred in ---------------------------------------- {typeof(TRequest).Name}\n {exception}");
            state.SetHandled(state.Response);
        }
    }
}