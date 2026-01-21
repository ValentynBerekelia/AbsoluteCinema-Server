using AbsoluteCinema.Domain.Primitives;
using AbsoluteCinema.Domain.ValueObjects;

namespace AbsoluteCinema.Domain.Entities;

public class User : AggregateRoot<UserId>
{
    public string UserName { get; private set; }
    public PasswordHash  PasswordHash { get; private set; }
    public string Email { get; private set; }
    
    private readonly List<Role> _roles = new List<Role>();
    public IReadOnlyCollection<Role> Roles => _roles;

    private User() { }

    private User(UserId id, string userName, PasswordHash passwordHash, string email)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
    }

    public static User Create(string userName, PasswordHash passwordHash, string email)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(userName));
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }
        if (!email.Contains("@"))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
        return new User(UserId.New(), userName, passwordHash, email);
    }

    public void ChangePassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }
        if (!email.Contains("@"))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }
        Email = email;
    }

    public void ChangeUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new ArgumentException("Username cannot be null or empty.", nameof(userName));
        }
        UserName = userName;
    }
    
    public void AddRole(Role role)
    {
        _roles.Add(role);
    }
    
    public void RemoveRole(Role role)
    {
        _roles.Remove(role);
    }
    
}

public record struct UserId(Guid Id)
{
    public static UserId New() => new UserId(Guid.NewGuid());
}

