using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Repositories;
using Microsoft.EntityFrameworkCore;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BloggieDbContext bloggieDbContext;

    public BlogPostRepository(BloggieDbContext bloggieDbContext)
    {
        this.bloggieDbContext = bloggieDbContext;
        
    }
    public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
            return await bloggieDbContext.BlogPosts
                .Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

    public async Task<BlogPost?> AddAsync(BlogPost blogPost)
    {
        await bloggieDbContext.BlogPosts.AddAsync(blogPost);
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
        return await bloggieDbContext.BlogPosts
            .Include(x => x.Tags)
            .ToListAsync();
    }

    public async Task<BlogPost?> GetAsync(Guid id)
    {
        return await bloggieDbContext.BlogPosts
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingBlog = await bloggieDbContext.BlogPosts
            .Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

        if (existingBlog != null)
        {
            existingBlog.Heading = blogPost.Heading;
            existingBlog.PageTitle = blogPost.PageTitle;
            existingBlog.Content = blogPost.Content;
            existingBlog.ShortDescription = blogPost.ShortDescription;
            existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingBlog.UrlHandle = blogPost.UrlHandle;
            existingBlog.PublishedDate = blogPost.PublishedDate;
            existingBlog.Author = blogPost.Author;
            existingBlog.Visible = blogPost.Visible;
            existingBlog.Tags = blogPost.Tags;

            await bloggieDbContext.SaveChangesAsync();
            return existingBlog;
        }
        return null;
    }
    public async Task<(IEnumerable<BlogPost>, int totalCount)> GetAllAsync(
        string searchString, 
        int pageNumber, 
        int pageSize, 
        string sortColumn, 
        string sortOrder)
    {
        var query = bloggieDbContext.BlogPosts
            .Include(x => x.Tags)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(b => 
                b.Heading.Contains(searchString) || 
                b.Content.Contains(searchString));
        }

        var totalCount = await query.CountAsync();

        // Apply sorting
        query = sortColumn?.ToLower() switch
        {
            "heading" => sortOrder == "asc" 
                ? query.OrderBy(b => b.Heading)
                : query.OrderByDescending(b => b.Heading),
            "publisheddate" => sortOrder == "asc"
                ? query.OrderBy(b => b.PublishedDate)
                : query.OrderByDescending(b => b.PublishedDate),
            _ => query.OrderByDescending(b => b.PublishedDate)
        };

        var posts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (posts, totalCount);
    }
}