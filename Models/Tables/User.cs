using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Models.Tables;

namespace backend.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }

    [Required]
    [MaxLength(50)]
    public string FullName { get; set; } = "Flashduo Guy";
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<UserExam> UserExams { get; set; } =  new();
    public List<FlashcardFolder> FlashcardFolders { get; set; } = new();
    public List<UserFlashcardSet> UserFlashcardSets { get; set; } = new();
    public List<FlashcardTest> FlashcardTests { get; set; } = new();
    public List<Flashcard> Flashcards { get; set; } = new();
    public List<UserFlashcardGame> UserFlashcardGames { get; set; } = new();
    
}
