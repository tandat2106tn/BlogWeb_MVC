using Bloggie.Web.Models.Domain;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bloggie.Web.Data
{
    public static class DbInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new BloggieDbContext(serviceProvider.GetRequiredService<DbContextOptions<BloggieDbContext>>()))
            {
                // Look for any products.
                if (context.Tags.Any())
                {
                    return;   // DB has been seeded
                }
                context.Tags.AddRange(
                    new Tag
                    {
                        Name = "Travel",
                        DisplayName = "Du lịch"
                    },
                    new Tag
                    {
                        Name = "Food",
                        DisplayName = "Ẩm thực"
                    },
                    new Tag
                    {
                        Name = "Technology",
                        DisplayName = "Công nghệ"
                    },
                    new Tag
                    {
                        Name = "Lifestyle",
                        DisplayName = "Lối sống"
                    },
                    new Tag
                    {
                        Name = "Entertainment",
                        DisplayName = "Giải trí"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
