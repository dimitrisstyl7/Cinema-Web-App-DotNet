using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("AppAdmin")]
[Index("UserId", Name = "UQ__AppAdmin__B9BE370E0982737A", IsUnique = true)]
public partial class AppAdmin
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AppAdmin")]
    public virtual User User { get; set; } = null!;
}
