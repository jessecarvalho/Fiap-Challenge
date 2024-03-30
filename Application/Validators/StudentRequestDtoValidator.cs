using Application.Dto;
using FluentValidation;

namespace Application.Validators;

public class StudentRequestDtoValidator : AbstractValidator<StudentRequestDto>
{
    public StudentRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(0, 255).WithMessage("Name must not exceed 255 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .Length(0, 255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .Length(0, 255).WithMessage("Username must not exceed 255 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
            .MaximumLength(255).WithMessage("Password must not be more than 255 characters long");
    }
}