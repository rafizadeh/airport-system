using MediatR;
using FluentValidation;

namespace AirportSystem.Application.Behaviour
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationContext = new ValidationContext<TRequest>(request);
            var validationResponse = await Task.WhenAll(validators.Select(x => x.ValidateAsync(validationContext)));

            var validationErrors = validationResponse.Where(x => x.Errors.Count != 0)
                .SelectMany(x => x.Errors)
                .ToList();

            if (validationErrors.Count != 0)
                throw new ValidationException(validationErrors);

            return await next();
        }
    }
}