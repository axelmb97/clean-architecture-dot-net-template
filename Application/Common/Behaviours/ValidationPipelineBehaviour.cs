using Application.Common.Dtos.Errors;
using Application.Common.Exceptions.Base;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours
{
    public class ValidationPipelineBehaviour<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse> 
            where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults
                            .SelectMany(x => x.Errors)
                            .Where(f => f != null)
                            .GroupBy(
                            x => x.PropertyName,
                            x => x.ErrorMessage,
                            (propertyName, errorMessages) => new
                            {
                                Key = propertyName,
                                Values = errorMessages.Distinct().ToArray()
                            })
                            .Select(x => new ValidationErrorDto { PropertyName = x.Key, Errors = x.Values })
                            .ToList();

            if (failures.Any())
            {
                throw new ValidationBaseException(failures);
            }

            return await next();
        }
    }
}
