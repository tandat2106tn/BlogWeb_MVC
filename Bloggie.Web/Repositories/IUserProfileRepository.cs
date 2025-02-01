using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bloggie.Web.Repositories
{
    public interface IUserProfileRepository
    {
        Task<UserProfile> GetByUserIdAsync(string userId);
        Task<UserProfile> AddAsync(UserProfile profile);
        Task<UserProfile> UpdateAsync(UserProfile profile);
        Task<bool> DeleteAsync(int id);
    }
}