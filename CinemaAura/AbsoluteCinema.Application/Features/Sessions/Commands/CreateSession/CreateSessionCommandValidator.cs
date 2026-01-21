using FluentValidation;

namespace AbsoluteCinema.Application.Features.Sessions.Commands.CreateSession;

public class CreateSessionCommandValidator : AbstractValidator<CreateSessionCommand>
{
    public CreateSessionCommandValidator()
    {
        RuleFor(x => x.MovieId).NotEmpty();
        RuleFor(x => x.HallId).NotEmpty();

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .GreaterThan(DateTime.UtcNow).WithMessage("Сеанс не може бути у минулому.");
        RuleForEach(x => x.Prices).ChildRules(price =>
        {
            price.RuleFor(p => p.SeatTypeId).NotEmpty();
            price.RuleFor(p => p.Price).GreaterThanOrEqualTo(0);
        });
    }
}