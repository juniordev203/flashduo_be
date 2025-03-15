using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Answer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int QuestionId { get; set; }

    [Required]
    public char OptionLabel { get; set; }  // A, B, C, D

    [Required]
    public string OptionText { get; set; }

    [ForeignKey("QuestionId")]
    public Question Question { get; set; }
}
