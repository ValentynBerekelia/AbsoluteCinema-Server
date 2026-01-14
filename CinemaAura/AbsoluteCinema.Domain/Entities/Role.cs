using CinemaAura.Domain.Primitives;
using CinemaAura.Domain.ValueObjects;

namespace AbsoluteCinema.Domain.Entities;

public class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; }
    
    private readonly HashSet<PermissionCode> _permissions = new HashSet<PermissionCode>();
    public IReadOnlyCollection<PermissionCode> Permissions => _permissions;
    
    private readonly HashSet<UserId> _userIds = new HashSet<UserId>();
    public IReadOnlyCollection<UserId> UserIds => _userIds;
    private Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Role Create(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(roleName));
        }
        return new Role(RoleId.New(), roleName);
    }
    
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Role name cannot be null or empty.", nameof(name));
        }
        Name = name;
    }
    
    public void Grant(PermissionCode permission)
    {
        _permissions.Add(permission);
    }
    
    public void Revoke(PermissionCode permission)
    {
        _permissions.Remove(permission);
    }

    public void RevokeAll()
    {
        _permissions.Clear();
    }

    public bool HasPermission(PermissionCode permission)
    {
        return _permissions.Contains(permission);
    }

    public void AddUser(UserId userId)
    {
        _userIds.Add(userId);
    }

    public void RemoveUser(UserId userId)
    {
        _userIds.Remove(userId);
    }
}

public record RoleId(Guid Id)
{
    public static RoleId New() => new RoleId(Guid.NewGuid());
} 