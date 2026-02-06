namespace AbsoluteCinema.Requests;

public record LoginUserRequest(
    string Email,
    string Password);
