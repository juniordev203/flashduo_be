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
    
    public DbSet<Exam> Exam { get; set; }
    public DbSet<ExamQuestion> ExamQuestion { get; set; }
    public DbSet<Question> Question { get; set; }
    public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
    public DbSet<UserAnswer> UserAnswer { get; set; }
    public DbSet<UserExam> UserExam { get; set; }
    
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
        
        // thiet lap quan he 
        modelBuilder.Entity<Question>()
            .Property(q => q.Part)
            .IsRequired()
            .HasConversion<int>();         
        modelBuilder.Entity<Question>()
            .Property(q => q.Section)
            .IsRequired()
            .HasConversion<int>(); 
        modelBuilder.Entity<UserExam>()
            .Property(ue => ue.Status)
            .IsRequired()
            .HasConversion<int>() 
            .HasDefaultValue(ExamStatus.NotStarted);
            
        // thiet lap khoa ngoai
        modelBuilder.Entity<UserExam>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserExam)
            .HasForeignKey(ue => ue.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ExamQuestion>()
            .HasOne(eq => eq.Exam)
            .WithMany(e => e.ExamQuestion)
            .HasForeignKey(eq => eq.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<QuestionAnswer>()
            .HasOne(c => c.Question)
            .WithMany(q => q.QuestionAnswer)
            .HasForeignKey(c => c.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.UserExam)
            .WithMany(ue => ue.UserAnswer)
            .HasForeignKey(ua => ua.UserExamId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.ExamQuestion)
            .WithMany()
            .HasForeignKey(ua => ua.ExamQuestionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}


