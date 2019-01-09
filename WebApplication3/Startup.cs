using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using WebApplication3.Models;

[assembly: OwinStartupAttribute(typeof(WebApplication3.Startup))]
namespace WebApplication3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createAdminUserAndApplicationRoles();
        }

        private void createAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new
           RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new
           UserStore<ApplicationUser>(context));
            // Se adauga rolurile aplicatiei
            if (!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
                // se adauga utilizatorul administrator
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                user.Name = "Administrator";
                var adminCreated = UserManager.Create(user, "Administrator1!");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Administrator");
                }
            }
            if (!roleManager.RoleExists("Organizer"))
            {
                var role = new IdentityRole();
                role.Name = "Organizer";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }
        }
    }
}
