using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggie.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly BloggieDbContext dbContext;

        public UserProfileRepository(BloggieDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserProfile> GetByUserIdAsync(string userId)
    {
        if (dbContext.UserProfiles == null)
        {
            return null;
        }
        return await dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

        public async Task<UserProfile> AddAsync(UserProfile profile)
        {
            await dbContext.UserProfiles.AddAsync(profile);
            await dbContext.SaveChangesAsync();
            return profile;
        }

        public async Task<UserProfile> UpdateAsync(UserProfile profile)
        {
            var existingProfile = await dbContext.UserProfiles.FindAsync(profile.Id);
            if (existingProfile != null)
            {
                dbContext.Entry(existingProfile).CurrentValues.SetValues(profile);
                await dbContext.SaveChangesAsync();
            }
            return existingProfile;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var profile = await dbContext.UserProfiles.FindAsync(id);
            if (profile != null)
            {
                dbContext.UserProfiles.Remove(profile);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}