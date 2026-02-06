namespace AbsoluteCinema.Requests;

public record RegisterUserRequest(
    string UserName,
    string Password,
    string Email);