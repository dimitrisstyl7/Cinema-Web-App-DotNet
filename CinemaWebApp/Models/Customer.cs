using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("Customer")]
[Index("UserId", Name = "UQ__Customer__B9BE370E8A6C7E48", IsUnique = true)]
public partial class Customer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey("UserId")]
    [InverseProperty("Customer")]
    public virtual User User { get; set; } = null!;
}
