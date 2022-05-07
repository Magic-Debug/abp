using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Volo.Blogging
{
    public interface IBlogUserRepository //:IUserRepository<IdentityUser>
    {
        Task<List<IdentityUser>> GetUsersAsync(int maxCount, string filter, CancellationToken cancellationToken = default);

        Task<IdentityUser> FindAsync(Guid userId);
    }
}