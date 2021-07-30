using FluentValidation;
using FluentValidation.Results;

namespace Theater.Application.Movies.Commands
{
    public class MovieAddCommand
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }

        public virtual ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }

        class Validator : AbstractValidator<MovieAddCommand>
        {
            public Validator()
            {
                RuleFor(m => m.Tittle).NotNull().NotEmpty().MinimumLength(1).MaximumLength(250).WithMessage("Título inválido");
                RuleFor(m => m.Description).NotNull().NotEmpty().MinimumLength(1).MaximumLength(250).WithMessage("Descrição inválida");
                RuleFor(m => m.Duration).NotNull().NotEmpty().GreaterThanOrEqualTo(1).LessThanOrEqualTo(1440).WithMessage("Tempo do filme inválido");
            }
        }
    }
}