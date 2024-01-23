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

    [Display(Name = "Movie Title")]
    [Column("movie_id")]
    public int MovieId { get; set; }

    [Display(Name = "Screening Room Name")]
    [Column("screening_room_id")]
    public int ScreeningRoomId { get; set; }

    [Display(Name = "Date and Time")]
    [Column("start_time", TypeName = "datetime")]
    public DateTime StartTime { get; set; }

    [Display(Name = "Number Of Remaining Seats")]
    [Column("remaining_no_of_seats")]
    public int RemainingNoOfSeats { get; set; }

    [Display(Name = "Movie Title")]
    [ForeignKey("MovieId")]
    [InverseProperty("Screenings")]
    public virtual Movie Movie { get; set; } = null!;

    [InverseProperty("Screening")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [Display(Name = "Screening Room")]
    [ForeignKey("ScreeningRoomId")]
    [InverseProperty("Screenings")]
    public virtual ScreeningRoom ScreeningRoom { get; set; } = null!;
}
