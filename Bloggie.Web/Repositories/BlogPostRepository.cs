using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await bloggieDbContext.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;


        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Tags = blogPost.Tags;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.Content = blogPost.Content;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;


        }

        public async Task<PaginatedList<BlogPost>> GetAllAsync(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            var blogPosts = bloggieDbContext.BlogPosts.Include(x => x.Tags).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                blogPosts = blogPosts.Where(s => s.Heading.Contains(searchString)
                    || s.PageTitle.Contains(searchString)
                    || s.Author.Contains(searchString));
            }

            blogPosts = sortOrder switch
            {
                "id_desc" => blogPosts.OrderByDescending(s => s.Id),
                "heading" => blogPosts.OrderBy(s => s.Heading),
                "heading_desc" => blogPosts.OrderByDescending(s => s.Heading),
                "publish_date" => blogPosts.OrderBy(s => s.PublishedDate),
                "publish_date_desc" => blogPosts.OrderByDescending(s => s.PublishedDate),
                _ => blogPosts.OrderBy(s => s.Id),
            };

            return await PaginatedList<BlogPost>.CreateAsync(blogPosts, pageNumber ?? 1, pageSize);
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
