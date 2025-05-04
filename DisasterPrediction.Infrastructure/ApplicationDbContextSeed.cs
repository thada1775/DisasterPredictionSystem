using DisasterPrediction.Domain.Entities;
using DisasterPrediction.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
    }
}
