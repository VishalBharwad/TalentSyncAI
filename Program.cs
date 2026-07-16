using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentSyncAI.Data;
using TalentSyncAI.Models.Identity;


namespace TalentSyncAI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { "Admin", "Recruiter", "Candidate" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));

                        if (result.Succeeded)
                        {
                            Console.WriteLine($"{role} created successfully.");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine(error.Description);
                            }
                        }
                    }
                }
            }



            using (var scope = app.Services.CreateScope())
            {
                // Role Manager
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = { "Admin", "Recruiter", "Candidate" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // User Manager
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string adminEmail = "admin@talentsync.com";
                string adminPassword = "Admin@123";

                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, adminPassword);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                        Console.WriteLine("Default Admin Created Successfully.");
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
