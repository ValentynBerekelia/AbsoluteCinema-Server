using CinemaAura.Domain.Primitives;
using CinemaAura.Domain.ValueObjects;

namespace IdentityService.Domain.Entities;

public class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; }
    
    private readonly HashSet<PermissionCode> _permissions = new HashSet<PermissionCode>();
    public IReadOnlyCollection<PermissionCode> Permissions => _permissions;
    
    //public List<User> Users { get; private set; }
    private Role(RoleId id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Role Create(string roleName)
    {
        return new Role(RoleId.New(), roleName);
    }
    
    public void Raname(string name)
    {
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

}

public record RoleId(Guid Id)
{
    public static RoleId New() => new RoleId(Guid.NewGuid());
} 