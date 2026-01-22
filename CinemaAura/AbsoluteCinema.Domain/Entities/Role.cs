using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; }

    private readonly List<Permission> _permissions = new List<Permission>();
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();

    private Role() { }
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

    public void Grant(Permission permission)
    {
        if (_permissions.All(p => p.Id != permission.Id))
            _permissions.Add(permission);
    }

    public void Revoke(Permission permission)
    {
        _permissions.Remove(permission);
    }

    public void RevokeAll()
    {
        _permissions.Clear();
    }

    public bool HasPermission(Permission permission)
    {
        return _permissions.Contains(permission);
    }
    
}

public record struct RoleId(Guid Id)
{
    public static RoleId New() => new RoleId(Guid.NewGuid());
}
