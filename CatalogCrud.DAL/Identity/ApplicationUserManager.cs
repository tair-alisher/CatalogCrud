using CatalogCrud.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace CatalogCrud.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }
    }
}
