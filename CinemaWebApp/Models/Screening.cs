﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("Screening")]
[Index("MovieId", "ScreeningRoomId", "StartTime", Name = "UQ__Screenin__3934ACA26E070CD5", IsUnique = true)]
public partial class Screening
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("movie_id")]
    public int MovieId { get; set; }

    [Column("screening_room_id")]
    public int ScreeningRoomId { get; set; }

    [Column("start_time", TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Column("remaining_no_of_seats")]
    public int RemainingNoOfSeats { get; set; }

    [ForeignKey("MovieId")]
    [InverseProperty("Screenings")]
    public virtual Movie Movie { get; set; } = null!;

    [InverseProperty("Screening")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey("ScreeningRoomId")]
    [InverseProperty("Screenings")]
    public virtual ScreeningRoom ScreeningRoom { get; set; } = null!;
}