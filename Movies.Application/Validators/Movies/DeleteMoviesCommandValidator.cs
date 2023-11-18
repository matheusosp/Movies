using FluentValidation;
using Movies.Application.Commands;

namespace Movies.Application.Validators.Movie
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
