namespace backend.Controllers.Flashcard.Response;

public class FlashcardResponse
{
    public int Id { get; set; }
    public string TermLanguage { get; set; }
    public string DefinitionLanguage { get; set; }
    public string ImageUrl { get; set; }
    public string AudioUrl { get; set; }
    public bool IsFavourite { get; set; }
    public bool IsDeleted { get; set; }
    public int CorrectCount { get; set; }
    public int WrongCount { get; set; }
    public DateTime? LastReviewedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FlashcardSetId { get; set; }
    public int UserId { get; set; }
}