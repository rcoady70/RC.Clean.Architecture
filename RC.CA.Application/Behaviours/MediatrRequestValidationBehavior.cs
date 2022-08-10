using System.Reflection;
using FluentValidation;
using MediatR;

namespace RC.CA.Application.Behaviours;
public class MediatrRequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                                                                       where TRequest : IRequest<TResponse>
                                                                       where TResponse : ICAResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public MediatrRequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                List<ValidationError> errorList = failures.ToCAResultErrors();
                MethodInfo methodInfo = typeof(TResponse).GetMethod("Invalid", new[] { typeof(List<ValidationError>) });
                //Execute none generic static method to create invalid CAResult<t> 
                var errorResponse = (TResponse)methodInfo.Invoke(null, new object[] { errorList });
                return await Task.FromResult(errorResponse);
            }
            else
                return await next();
        }
        return await next();
    }
}

