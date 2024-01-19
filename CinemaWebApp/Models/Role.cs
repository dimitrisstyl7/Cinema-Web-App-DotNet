using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("Role")]
[Index("Name", Name = "UQ__Role__72E12F1B7E96D9B6", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(20)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
