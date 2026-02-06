using AbsoluteCinema.Application.Repository;
using MediatR;

namespace AbsoluteCinema.Application.Features.Auth;

public class CreateUserCommandHandler(ICreateUserCommand createUser) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly ICreateUserCommand _createUser = createUser;

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken ct)
    {
        return await _createUser.ExecuteAsync(request, ct);
    }
}

public record CreateUserCommand : IRequest<CreateUserResponse>
{
    public string UserName { get; init; }
    public string Password { get; init; }
    public string Email { get; init; }
}

public record CreateUserResponse(Guid UserId, string AccessToken, string RefreshToken)
{
}