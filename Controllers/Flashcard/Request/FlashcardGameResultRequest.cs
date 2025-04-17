namespace backend.Controllers.Flashcard.Request;

public class FlashcardGameResultRequest
{
    public int UserId { get; set; }
    public int SetId { get; set; }
    public int TotalWord { get; set; }
    public double DurationTime { get; set; }
}