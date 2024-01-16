using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("Cinema")]
[Index("Email", Name = "UQ__Cinema__AB6E6164CE3A75E4", IsUnique = true)]
[Index("Name", "Address", "City", "ZipCode", Name = "UQ__Cinema__F8DCF1A95BA0DAC1", IsUnique = true)]
public partial class Cinema
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(50)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [Column("address")]
    [StringLength(50)]
    [Unicode(false)]
    public string Address { get; set; } = null!;

    [Column("city")]
    [StringLength(50)]
    [Unicode(false)]
    public string City { get; set; } = null!;

    [Column("zip_code")]
    [StringLength(10)]
    [Unicode(false)]
    public string ZipCode { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("no_of_screening_rooms")]
    public int NoOfScreeningRooms { get; set; }

    [InverseProperty("Cinema")]
    public virtual ICollection<ContentAdmin> ContentAdmins { get; set; } = new List<ContentAdmin>();

    [InverseProperty("Cinema")]
    public virtual ICollection<ScreeningRoom> ScreeningRooms { get; set; } = new List<ScreeningRoom>();
}
