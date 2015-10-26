using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;

namespace Hostsol.TFSTB.WebFrontend.Models
{
    public class SeedData
    {
        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider, bool createUsers = true)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                await db.Database.EnsureCreatedAsync();
                await CreateAdminUser(serviceProvider);
            }
        }

        /// <summary>
        /// Creates a store manager user who can manage the inventory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var appEnv = serviceProvider.GetService<IApplicationEnvironment>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            var configuration = builder.Build();
            
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            
            if (roleManager.Roles.Count<ApplicationRole>() == 0 || !await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole("Admin"));
            }
            var user = await userManager.FindByNameAsync(configuration.GetSection("AppSettings").GetSection("defaultAdminUserName").Value);
            if (user == null)
            {
                user = new ApplicationUser { UserName = configuration.GetSection("AppSettings").GetSection("defaultAdminUserName").Value };
                await userManager.CreateAsync(user, configuration.GetSection("AppSettings").GetSection("defaultAdminPassword").Value);
                //await userManager.AddToRoleAsync(user, adminRole);
                await userManager.AddClaimAsync(user, new Claim("ManageStore", "Allowed"));
            }
        }
    }
}
