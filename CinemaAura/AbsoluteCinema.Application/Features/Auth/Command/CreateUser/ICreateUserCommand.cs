namespace AbsoluteCinema.Application.Features.Auth;

public interface ICreateUserCommand
{
    Task<CreateUserResponse> ExecuteAsync(CreateUserCommand request, CancellationToken ct);
}