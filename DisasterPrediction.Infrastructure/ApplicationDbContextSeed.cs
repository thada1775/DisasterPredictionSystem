using DisasterPrediction.Domain.Entities;
using DisasterPrediction.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DisasterPrediction.Infrastructure
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = SystemConstant.UserRole.GetAllRole();

            foreach (var roleName in roles)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }

                var SystemConstant = GetSystemConstantForRole(roleName);
                foreach (var permission in SystemConstant)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name!) || !(await roleManager.GetClaimsAsync(role)).Any(c => c.Type == "Permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }
                }
            }

            var adminUser = new ApplicationUser { UserName = "thada.fordev@gmail.com", Email = "thada.fordev@gmail.com", EmailConfirmed = true };
            var password = "Secret123!";

            var existAdmin = await userManager.FindByEmailAsync(adminUser.Email);

            if (existAdmin == null)
            {
                var result = await userManager.CreateAsync(adminUser, password);
                existAdmin = await userManager.FindByEmailAsync(adminUser.Email);
            }
            if (existAdmin != null && !(await userManager.IsInRoleAsync(existAdmin, SystemConstant.UserRole.Admin)))
            {
                await userManager.AddToRoleAsync(existAdmin, SystemConstant.UserRole.Admin);
            }
        }

        private static List<string> GetSystemConstantForRole(string role)
        {
            return role switch
            {
                "Admin" => new List<string>
                {
                    //SystemConstant.User.AllPermission,
                    //SystemConstant.Project.View, SystemConstant.Project.Create, SystemConstant.Project.Edit, SystemConstant.Project.Delete,
                    //SystemConstant.Task.View, SystemConstant.Task.Create, SystemConstant.Task.Edit, SystemConstant.Task.Delete
                },
                "Manager" => new List<string>
                {
                    //SystemConstant.Project.View, SystemConstant.Project.Create, SystemConstant.Project.Edit, SystemConstant.Project.Delete,
                    //SystemConstant.Task.View, SystemConstant.Task.Create, SystemConstant.Task.Edit, SystemConstant.Task.Delete
                },
                "User" => new List<string>
                {
                    //SystemConstant.Project.View, SystemConstant.Project.Create, SystemConstant.Project.Edit, SystemConstant.Project.Delete,
                    //SystemConstant.Task.View, SystemConstant.Task.Create, SystemConstant.Task.Edit, SystemConstant.Task.Delete
                },
                _ => new List<string>()
            };
        }

    }
}
