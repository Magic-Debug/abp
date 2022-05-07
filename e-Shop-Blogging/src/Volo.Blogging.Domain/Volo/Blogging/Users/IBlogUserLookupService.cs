using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Volo.Blogging.Users
{
    public interface IBlogUserLookupService : IUserLookupService<IdentityUser>
    {

    }
}
