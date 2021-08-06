using FluentValidation;
using MediatR;
using System;

namespace Theater.Application.Sections.Commands
{
    public class SectionAddCommand : IRequest<bool>
    {
        public string MovieTittle { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Value { get; set; }
        public string AnimationType { get; set; }
        public int AudioType { get; set; }
    }

    public class SectionAddCommandValidator : AbstractValidator<SectionAddCommand>
    {
        public SectionAddCommandValidator()
        {
            RuleFor(m => m.MovieTittle)
                .NotEmpty().WithMessage("Filme inválido")
                .MinimumLength(1).WithMessage("Filme inválido")
                .MaximumLength(250).WithMessage("Filme inválido");
            RuleFor(m => m.RoomName)
                .NotEmpty().WithMessage("Sala inválida")
                .MinimumLength(1).WithMessage("Sala inválida")
                .MaximumLength(250).WithMessage("Sala inválida");
            RuleFor(m => m.StartDate)
                .NotEmpty().WithMessage("Horário da seção inválido")
                .GreaterThan(DateTime.Now).WithMessage("Horário da seção inválido");
            RuleFor(m => m.Value)
                .NotEmpty().WithMessage("Valor da seção inválido")
                .GreaterThan(1).WithMessage("Valor da seção inválido")
                .LessThan(9999).WithMessage("Valor da seção inválido");
            RuleFor(m => m.AnimationType)
                .NotNull().WithMessage("Tipo de animação inválido")
                .NotEmpty().WithMessage("Tipo de animação inválido")
                .Must(x => x.Equals("2D", StringComparison.InvariantCultureIgnoreCase) ||
                           x.Equals("3D", StringComparison.InvariantCultureIgnoreCase)).WithMessage("Tipo de animação inválido");
            RuleFor(m => m.AudioType)
                .NotNull().WithMessage("Tipo de audio inválido")
                .NotEmpty().WithMessage("Tipo de audio inválido")
                .Must(x => x == 1 || x == 2).WithMessage("Tipo de audio inválido");
        }
    }
}
