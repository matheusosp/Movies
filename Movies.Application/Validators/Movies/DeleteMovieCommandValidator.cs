using FluentValidation;
using Movies.Application.Commands.Movies;

namespace Movies.Application.Validators.Movies
{
    public class DeleteMovieCommandValidator : AbstractValidator<DeleteMovieCommand>
    {
        public DeleteMovieCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("{PropertyName} deve ser maior que 0");
        }
    }
}
