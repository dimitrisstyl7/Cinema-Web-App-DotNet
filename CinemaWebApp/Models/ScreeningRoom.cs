using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("ScreeningRoom")]
[Index("CinemaId", "Name", Name = "UQ__Screenin__E14C9588C7BE0EA7", IsUnique = true)]
public partial class ScreeningRoom
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Display(Name = "Cinema Name")]
    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Display(Name = "Screening Room Name")]
    [Column("name")]
    [StringLength(20)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Display(Name = "Total Number of Seats")]
    [Column("total_no_of_seats")]
    public int TotalNoOfSeats { get; set; }

    [Display(Name = "3D")]
    [Column("3D")]
    public bool _3d { get; set; }

    [ForeignKey("CinemaId")]
    [InverseProperty("ScreeningRooms")]
    public virtual Cinema Cinema { get; set; } = null!;

    [InverseProperty("ScreeningRoom")]
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
