using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Identity;

namespace Volo.Blogging
{
    public class EfCoreBlogUserRepository :IBlogUserRepository
    {
        public EfCoreBlogUserRepository()
        {

        }

        public Task<IdentityUser> FindAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public  Task<List<IdentityUser>> GetUsersAsync(int maxCount, string filter, CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}
