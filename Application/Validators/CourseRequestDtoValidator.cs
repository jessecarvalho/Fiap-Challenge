using Application.Dto;
using Application.DTOs;
using Core.Enums;
using FluentValidation;
using Infrastructure.Interfaces.Repositories;

namespace Application.Validators;

public class CourseRequestDtoValidator : AbstractValidator<CourseRequestDto>
{
    public CourseRequestDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .Length(0, 255).WithMessage("Title must not exceed 255 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(0, 500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Year)
            .IsInEnum().WithMessage("Year is not valid");

    }
    
}