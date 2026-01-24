using FluentValidation;
using MediatR;

namespace AbsoluteCinema.Application.Common
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // no validators, continue to the next behavior/handler in the pipeline
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            // start all validations in parallel (do not use async operation like call to DB in validators - anti-pattern)
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Count != 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
                throw new ValidationException(failures);

            return await next(cancellationToken);
        }
    }
}