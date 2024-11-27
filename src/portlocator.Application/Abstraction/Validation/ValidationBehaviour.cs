using FluentValidation;
using MediatR;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Abstraction.Validation
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var validationFailures = _validators.Select(v => v.Validate(request))
                                                .SelectMany(result => result.Errors)
                                                .Where(f => f != null)
                                                .ToList();

            if (validationFailures.Count > 0)
            {
                var errors = validationFailures.Select(x => new
                                               {
                                                   Name = x.PropertyName,
                                                   Message = x.ErrorMessage,
                                               })
                                               .Cast<object>()
                                               .ToList();

                var errorDetails = new Error(400, "Validation Error")
                {
                    ErrorDetails = errors
                };

                var errorResponse = Result.BadRequest<TResponse>(default, errorDetails);

                return (TResponse)(object)errorResponse;
            }

            return await next();
        }
    }
}
