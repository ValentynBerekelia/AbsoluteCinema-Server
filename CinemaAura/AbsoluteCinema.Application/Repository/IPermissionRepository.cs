using AbsoluteCinema.Application.Abstructions;
using AbsoluteCinema.Domain.Entities;

namespace AbsoluteCinema.Application.Repository;

public interface IPermissionRepository : IRepository<PermissionId, Permission>
{
}
