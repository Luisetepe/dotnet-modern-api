using FluentValidation;
using GameStore.Api.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Http;

namespace GameStore.Api.SharedKernel.Extensions;

public static class ExceptionExtensions
{
    public static IResult MapToApiResult(this Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => Results.BadRequest(new 
            {
                Message = "One or more validation errors occurred.",
                Errors = validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        group => char.ToLowerInvariant(group.Key[0]) + group.Key[1..],
                        group => group.Select(error => error.ErrorMessage).ToArray()
                    )
            }),
            NotFoundException notFoundException => Results.NotFound(new
            {
                Message = notFoundException.Message
            }),
            _ => Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = 500,
                Title = "Something went wrong.",
                #if DEBUG
                Detail = exception.Message
                #else
                Detail = "An unrecoverable error occurred while processing your request. Please try again later."
                #endif
            })
        };
    }
}