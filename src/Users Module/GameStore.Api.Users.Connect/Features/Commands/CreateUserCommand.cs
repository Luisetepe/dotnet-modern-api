using FluentValidation;
using LanguageExt.Common;
using MediatR;

namespace GameStore.Api.Users.Connect.Features.Commands;

public record CreateUserCommand : IRequest<Result<CreateUserResponse>>
{
    public record UserAddress
    {
        public string Street { get; init; }
        public string City { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }
    }
    
    public string Name { get; init; }
    public string Email { get; init; }
    public UserAddress Address { get; init; }
    public DateOnly BirthDate { get; init; }
}

public record CreateUserResponse
{
    public string UserId { get; init; }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(150);
        RuleFor(x => x.Address).NotNull();
        RuleFor(x => x.Address.Street).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address.City).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address.Country).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Address.ZipCode).NotEmpty().MaximumLength(150);
        RuleFor(x => x.BirthDate).NotNull()
            .Must(x =>
            {
                var now = DateOnly.FromDateTime(DateTime.UtcNow);
                return x < now;
            })
            .WithMessage("Birth date cannot be in the future.");
    }
}

