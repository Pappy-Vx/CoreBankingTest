using CoreBankingTest.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CoreBankingTest.Application.Common.Behaviors
{
    public class ValidaitonBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidaitonBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            
          return await next();
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Any())
                {
                    // Create failure result
                    var resultType = typeof(TResponse);
                    if (resultType.IsGenericType)
                    {
                        var genericType = resultType.GetGenericArguments()[0];
                        var failureMethod = typeof(Result<>).MakeGenericType(genericType).GetMethod(nameof(Result<object>.Failure))!;

                        var errors = failures.Select(f => f.ErrorMessage).ToArray();
                        return (TResponse)failureMethod.Invoke(null, new object[] { errors })!;
                    }

                }
                return await next();
            }
        }
    }

    

