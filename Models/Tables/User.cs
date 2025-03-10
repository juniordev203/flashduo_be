using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }
    
    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
}
