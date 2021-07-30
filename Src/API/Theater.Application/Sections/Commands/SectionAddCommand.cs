using FluentValidation;
using FluentValidation.Results;
using System;

namespace Theater.Application.Sections.Commands
{
    public class SectionAddCommand
    {
        public string MovieTittle { get; set; }
        public string RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Value { get; set; }
        public string AnimationType { get; set; }
        public int AudioType { get; set; }

        public virtual ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }

        class Validator : AbstractValidator<SectionAddCommand>
        {
            public Validator()
            {
                RuleFor(m => m.MovieTittle).NotNull().NotEmpty().MinimumLength(1).MaximumLength(250).WithMessage("Título do filme inválido");
                RuleFor(m => m.RoomName).NotNull().NotEmpty().MinimumLength(1).MaximumLength(250).WithMessage("Nome da sala inválido");
                RuleFor(m => m.StartDate).NotNull().NotEmpty().GreaterThan(DateTime.Now).WithMessage("Horário da seção inválido");
                RuleFor(m => m.Value).NotNull().NotEmpty().GreaterThan(1).LessThan(9999).WithMessage("Valor da seção inválido");
                RuleFor(m => m.AnimationType).NotNull().NotEmpty().Must(x => x.Equals("2D", StringComparison.InvariantCultureIgnoreCase) || 
                                              x.Equals("3D", StringComparison.InvariantCultureIgnoreCase)).WithMessage("Tipo de animação inválido");
                RuleFor(m => m.AudioType).NotNull().NotEmpty().Must(x => x == 1 || x == 2 ).WithMessage("Tipo de audio inválido");
            }
        }
    }
}
