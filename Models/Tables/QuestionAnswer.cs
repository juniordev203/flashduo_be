using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class QuestionAnswer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    [Required]
    public string OptionLabel { get; set; }  // A, B, C, D

    [Required]
    public string OptionText { get; set; }
    [Required]
    public bool IsAnswer { get; set; }

    [ForeignKey("QuestionId")]
    public Question? Question { get; set; }
}
