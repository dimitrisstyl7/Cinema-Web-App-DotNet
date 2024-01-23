using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("Reservation")]
[Index("CustomerId", "ScreeningId", Name = "UQ__Reservat__E912BFE88F4C5DCA", IsUnique = true)]
public partial class Reservation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("screening_id")]
    public int ScreeningId { get; set; }

    [Column("no_of_booked_seats")]
    public int NoOfBookedSeats { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reservations")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("ScreeningId")]
    [InverseProperty("Reservations")]
    public virtual Screening Screening { get; set; } = null!;
}
