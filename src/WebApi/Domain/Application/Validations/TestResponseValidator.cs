using Api.Test.Domain.Application.Models;
using FluentValidation;

namespace Api.Test.Domain.Application.Validations;

public class TestResponseValidator : AbstractValidator<Login> 
{
    public TestResponseValidator()
    {
        RuleFor(p => p.UserName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("UserName|El campo UserName es un dato obligatorio")
            .NotNull().WithMessage("UserName|El campo UserName es un dato obligatorio");

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password|El campo Password es obligatorio")
            .NotNull().WithMessage("Password|El campo Password es obligatorio")
            .Length(5, 12)
            .WithMessage("Password|La longitud del campo Password debe ser de 5 u 12 caracteres")
            .Matches(@"\b[A-Z]\w*\b")
            .WithMessage("Password|El campo Password tiene un formato no válido");
    }
}
