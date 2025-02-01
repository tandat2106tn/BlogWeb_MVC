using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
        Task<IdentityUser> GetByIdAsync(string id);
        Task<bool> UpdateUserAsync(string id, string username, string email);
        Task<bool> ChangePasswordAsync(string id, string currentPassword, string newPassword);
    }
}
