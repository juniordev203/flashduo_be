namespace backend.Controllers.Flashcard.Response;

public class FlashcardResponse
{
    public int Id { get; set; }
    public string TermLanguage { get; set; }
    public string DefinitionLanguage { get; set; }
    public string ImageUrl { get; set; }
    public string AudioUrl { get; set; }
    public int FlashcardSetId { get; set; }
    public int UserId { get; set; }
}