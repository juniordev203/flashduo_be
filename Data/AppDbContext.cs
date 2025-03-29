using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<ForumTopic> ForumTopics { get; set; }
    public DbSet<ForumPost> ForumPosts { get; set; }
    public DbSet<ForumComment> ForumComments { get; set; }
    public DbSet<ForumEmotion> ForumEmotions { get; set; }
    public DbSet<ForumCategory> ForumCategories { get; set; }
    
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public DbSet<UserExam> UserExams { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithOne(u => u.Account)
            .HasForeignKey<User>(u => u.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExamQuestion>()
            .HasOne(eq => eq.Exam)
            .WithMany(e => e.ExamQuestions)
            .HasForeignKey(eq => eq.ExamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<QuestionAnswer>()
            .HasOne(c => c.Question)
            .WithMany(q => q.QuestionAnswers)
            .HasForeignKey(c => c.QuestionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.UserExam)
            .WithMany(ue => ue.UserAnswers)
            .HasForeignKey(ua => ua.UserExamId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<UserAnswer>()
            .HasOne(ua => ua.ExamQuestion)
            .WithMany(eq => eq.UserAnswers)
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserExam>()
            .Property(ue => ue.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ExamStatus.NotStarted);
    }
}



