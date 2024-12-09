using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Data
{
    public class AuthDbContext : IdentityDbContext

    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Seed roles (user-admin-super admin)
            var userRoleId = "05760998 - 161f - 489a - a7d4 - 2e8949a4079c";
            var adminRoleId = "973f8c44-a192-4c91-914d-e783b35a9910";
            var adminSuperRoleId = "8064eb1c-3bd3-4579-874c-843c64b98a64";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id= adminRoleId,
                    ConcurrencyStamp = adminRoleId

                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = adminSuperRoleId,
                    ConcurrencyStamp = adminSuperRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);


            //seed super admin
            var superAdminId = "a0740e33-120b-446d-afff-a1aedd039d84";
            var superAdminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "tandat2106tn@gmail.com",
                NormalizedEmail = "tandat2106tn@gmail.com".ToUpper(),
                NormalizedUserName = "admin".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "tandat2106Aa!");

            builder.Entity<IdentityUser>().HasData(superAdminUser);
            var superAdminRoles = new List<IdentityUserRole<String>>
            {
                new IdentityUserRole<string>
                {
                    RoleId= adminSuperRoleId,
                    UserId= superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId= adminRoleId,
                    UserId= superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId= userRoleId,
                    UserId= superAdminId
                }
            };
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);



        }
    }
}
