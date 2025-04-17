namespace backend.Controllers.Flashcard.Response;

public class FlashcardGameResultByUserResponse
{
    public int Id { get; set; }
    public string SetName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int DurationTime { get; set; }
}