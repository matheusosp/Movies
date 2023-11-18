using FluentValidation;
using Movies.Application.Commands;
using Movies.Application.Commands.Gender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators.Genders
{
    public class AddGenderCommandValidator : AbstractValidator<AddGenderCommand>
    {
        public AddGenderCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 100).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} characters");


        }
    }
}
