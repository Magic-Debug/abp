using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Blogging.Users;

public class BlogUserLookupService :IBlogUserLookupService
{
    public BlogUserLookupService(
        IBlogUserRepository userRepository,
        IUnitOfWorkManager unitOfWorkManager)

    {
        
    }

    public Task<IdentityUser> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<IUserData>> SearchAsync(string sorting = null, string filter = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
