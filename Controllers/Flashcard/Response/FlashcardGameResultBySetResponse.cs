namespace backend.Controllers.Flashcard.Response;

public class FlashcardGameResultBySetResponse
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public double DurationTime { get; set; }
}