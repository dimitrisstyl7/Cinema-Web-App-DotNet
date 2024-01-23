using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("ContentAdmin")]
[Index("UserId", "CinemaId", Name = "UQ__ContentA__2CD81F793BC62893", IsUnique = true)]
public partial class ContentAdmin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [ForeignKey("CinemaId")]
    [InverseProperty("ContentAdmins")]
    public virtual Cinema Cinema { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ContentAdmins")]
    public virtual User User { get; set; } = null!;
}
