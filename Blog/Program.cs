using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            try
            {
                var scpoe = host.Services.CreateScope();

                var context = scpoe.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scpoe.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scpoe.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                if (!context.Roles.Any())
                {
                    roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                }

                if (!context.Users.Any(u => u.UserName == "admin"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@test.com"
                    };

                    userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
