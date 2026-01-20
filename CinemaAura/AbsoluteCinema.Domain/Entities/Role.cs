using AbsoluteCinema.Domain.Primitives;

namespace AbsoluteCinema.Domain.Entities;

public class Role : AggregateRoot<RoleId>
{
    public string Name { get; private set; }

    private readonly HashSet<RolePermission> _permissionsIds = new HashSet<RolePermission>();
    public IReadOnlyCollection<RolePermission> PermissionsIds => _permissionsIds;

    private readonly HashSet<UserId> _userIds = new HashSet<UserId>();
    public IReadOnlyCollection<UserId> UserIds => _userIds;
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

    public void Grant(PermissionId permissionId)
    {
        _permissionsIds.Add(new RolePermission(permissionId));
    }

    public void Revoke(PermissionId permissionId)
    {
        _permissionsIds.Remove(new RolePermission(permissionId));
    }

    public void RevokeAll()
    {
        _permissionsIds.Clear();
    }

    public bool HasPermission(PermissionId permissionId)
    {
        return _permissionsIds.Contains(new RolePermission(permissionId));
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

public record struct RoleId(Guid Id)
{
    public static RoleId New() => new RoleId(Guid.NewGuid());
}

public record RolePermission(PermissionId PermissionId);