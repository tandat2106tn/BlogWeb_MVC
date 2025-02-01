using Bloggie.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authDbContext;
        private readonly UserManager<IdentityUser> userManager;

        public UserRepository(AuthDbContext authDbContext, UserManager<IdentityUser> userManager)
        {
            this.authDbContext = authDbContext;
            this.userManager = userManager;
        }
        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await authDbContext.Users.ToListAsync();
            var superAdminUser = await authDbContext.Users.FirstOrDefaultAsync(x => x.Email == "tandat2106tn@gmail.com");
            if (superAdminUser != null)
            {
                users.Remove(superAdminUser);
            }
            return users;

        }
        public async Task<IdentityUser> GetByIdAsync(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(string id, string username, string email)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            user.UserName = username;
            user.Email = email;
            user.NormalizedUserName = username.ToUpper();
            user.NormalizedEmail = email.ToUpper();

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string id, string currentPassword, string newPassword)
        {
            var user = await GetByIdAsync(id);
            if (user == null) return false;

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }
    }
}
