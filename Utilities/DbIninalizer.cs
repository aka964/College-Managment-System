using Microsoft.AspNetCore.Identity;

namespace SchoolManagementApp.MVC.Utilities
{
    public class DbIninalizer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                var roles = new[] { StaticData.role_admin, StaticData.role_cust };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                string adminEmail = "admin@admin.com";
                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var user = new IdentityUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail
                    };
                    var result = await userManager.CreateAsync(user, "Admin@123");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, StaticData.role_admin);
                }
            }
        }
    }
}
