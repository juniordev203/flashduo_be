namespace backend.Controllers.Flashcard.Response;

public class FlashcardSetResponse
{
    public int Id { get; set; }
    public string SetName { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public int FolderId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalCards { get; set; }
}