using FluentValidation;
using Movies.Application.Commands.Genre;

namespace Movies.Application.Validators.Genres
{
    public class AddGenreCommandValidator : AbstractValidator<AddMovieGenreCommand>
    {
        public AddGenreCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Propriedade {PropertyName} deve estar preenchida.")
                .Length(2, 100).WithMessage("{PropertyName} deve ter entre {MinLength} e {MaxLength} characters");


        }
    }
}
