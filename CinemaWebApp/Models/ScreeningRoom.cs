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

    [Column("cinema_id")]
    public int CinemaId { get; set; }

    [Column("name")]
    [StringLength(20)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("total_no_of_seats")]
    public int TotalNoOfSeats { get; set; }

    [Column("3D")]
    public bool _3d { get; set; }

    [ForeignKey("CinemaId")]
    [InverseProperty("ScreeningRooms")]
    public virtual Cinema Cinema { get; set; } = null!;

    [InverseProperty("ScreeningRoom")]
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
