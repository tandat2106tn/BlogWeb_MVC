using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

public class BloggieDbContext : DbContext
{
    public BloggieDbContext(DbContextOptions<BloggieDbContext> options) : base(options)
    {
    }

    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<BlogPost>()
            .HasMany(x => x.Tags)
            .WithMany(x => x.BlogPosts);

        modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.UserId)
            .IsUnique();

		modelBuilder.Entity<UserProfile>(entity =>
    {
        entity.HasIndex(e => e.UserId).IsUnique();
        entity.Property(e => e.UserId).IsRequired();
        entity.Property(e => e.FullName).IsRequired();
    });
    }
	
	
}