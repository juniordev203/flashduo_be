using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> User { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<ForumTopic> ForumTopic { get; set; }
    public DbSet<ForumPost> ForumPost { get; set; }
    public DbSet<ForumComment> ForumComment { get; set; }
    public DbSet<ForumEmotion> ForumEmotion { get; set; }
    public DbSet<ForumCategory> ForumCategory { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cấu hình quan hệ 1-1 giữa Account và User
        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithOne(u => u.Account)
            .HasForeignKey<User>(u => u.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ForumTopic>()
            .HasOne(a => a.ForumCategory)
            .WithMany(t => t.Topics)
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ForumPost>()
            .HasOne(p => p.ForumTopic)
            .WithMany(t => t.Posts)
            .HasForeignKey(p => p.TopicId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ForumComment>()
            .HasOne(c => c.ForumPost)
            .WithMany(p => p.Comments)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ForumEmotion>()
            .HasOne(e => e.ForumPost)
            .WithMany(p => p.Emotions)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //quan he giua account - post, comment, emotion
        modelBuilder.Entity<ForumPost>()
            .HasOne(p => p.Account)
            .WithMany(a => a.Posts)
            .HasForeignKey(p => p.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ForumComment>()
            .HasOne(c => c.Account)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.AccountId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ForumEmotion>()
            .HasOne(e => e.Account)
            .WithMany(a => a.Emotions)
            .HasForeignKey(e => e.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

    }

}


