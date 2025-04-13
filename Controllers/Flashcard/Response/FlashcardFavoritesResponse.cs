namespace backend.Controllers.Flashcard.Response;

public class FlashcardFavoritesResponse
{
    public int Id { get; set; }
    public string TermLanguage { get; set; }
    public string DefinitionLanguage { get; set; }
    public string ImageUrl { get; set; }
    public string AudioUrl { get; set; }
    public int FlashcardSetId { get; set; }
    public Boolean IsFavorite { get; set; }
}