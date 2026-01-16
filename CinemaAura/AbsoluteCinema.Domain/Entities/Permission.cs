using CinemaAura.Domain.Primitives;
using CinemaAura.Domain.ValueObjects;

namespace AbsoluteCinema.Domain.Entities;

public class Permission : Entity<PermissionId>
{
    public PermissionCode Code { get; private set; }

    private Permission() { }

    private Permission(PermissionId id, PermissionCode code)
    {
        Id = id;
        Code = code;
    }

    public static Permission Create(PermissionCode code)
    {
        return new Permission(PermissionId.New(), code);
    }
}

public record struct PermissionId(Guid Id)
{
    public static PermissionId New() => new PermissionId(Guid.NewGuid());
}