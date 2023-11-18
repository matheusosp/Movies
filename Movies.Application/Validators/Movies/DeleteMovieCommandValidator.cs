using FluentValidation;

namespace Movies.Application.Validators.Movie
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
