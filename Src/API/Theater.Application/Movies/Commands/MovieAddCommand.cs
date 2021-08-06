using FluentValidation;
using MediatR;

namespace Theater.Application.Movies.Commands
{
    public class MovieAddCommand : IRequest<bool>
    {
        public string Tittle { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
    }

    public class MovieAddCommandValidator : AbstractValidator<MovieAddCommand>
    {
        public MovieAddCommandValidator()
        {
            RuleFor(m => m.Tittle)
                .NotEmpty().WithMessage("Título inválido")
                .MinimumLength(1).WithMessage("Título inválido")
                .MaximumLength(250).WithMessage("Título inválido");
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Descrição inválida")
                .MinimumLength(1).WithMessage("Descrição inválida")
                .MaximumLength(250).WithMessage("Descrição inválida");
            RuleFor(m => m.Duration)
                .NotEmpty().WithMessage("Duração do filme inválida")
                .GreaterThanOrEqualTo(1).WithMessage("Duração do filme inválida")
                .LessThanOrEqualTo(1440).WithMessage("Duração do filme inválida");
        }
    }
}