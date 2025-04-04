namespace backend.Controllers.Flashcard.Response;

public class FlashcardSetDetailResponse
{
    public int Id { get; set; }
    public string SetName { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public int FolderId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<FlashcardResponse> Flashcards { get; set; }
}
