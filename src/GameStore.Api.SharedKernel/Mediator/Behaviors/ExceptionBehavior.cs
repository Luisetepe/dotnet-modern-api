using FluentValidation;
using FluentValidation.Results;
using LanguageExt.Common;
using MediatR;

namespace GameStore.Api.SharedKernel.Mediator.Behaviors;

public sealed class ExceptionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            return (TResponse)Activator.CreateInstance(typeof(TResponse), [ex])!;
        }
    }
}