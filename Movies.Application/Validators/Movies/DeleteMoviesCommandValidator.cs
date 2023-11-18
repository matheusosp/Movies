using FluentValidation;
using Movies.Application.Commands.Movies;

namespace Movies.Application.Validators.Movies
{
    public class DeleteMoviesCommandValidator : AbstractValidator<DeleteMoviesCommand>
    {
        public DeleteMoviesCommandValidator()
        {
            RuleFor(c => c.Ids)
                .ForEach(id => id.GreaterThan(0).WithMessage("Cada ID deve ser maior que 0"));
        }
    }
}
