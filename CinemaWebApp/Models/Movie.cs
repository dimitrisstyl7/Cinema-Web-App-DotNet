﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CinemaWebApp.Models;

[Table("Movie")]
public partial class Movie
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("content_admin_id")]
    public int ContentAdminId { get; set; }

    [Display(Name = "Genre")]
    [Column("genre_id")]
    public int GenreId { get; set; }

    [Column("title")]
    [StringLength(50)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Display(Name = "Duration (in minutes)")]
    [Column("duration")]
    public int Duration { get; set; }

    [Column("content")]
    [Unicode(false)]
    public string Content { get; set; } = null!;

    [Column("description")]
    [Unicode(false)]
    public string Description { get; set; } = null!;

    [Display(Name = "Release Year")]
    [Column("release_date")]
    [StringLength(4)]
    [Unicode(false)]
    public string ReleaseDate { get; set; } = null!;

    [Column("director")]
    [StringLength(50)]
    [Unicode(false)]
    public string Director { get; set; } = null!;

    [ForeignKey("ContentAdminId")]
    [InverseProperty("Movies")]
    public virtual ContentAdmin ContentAdmin { get; set; } = null!;

    [Display(Name = "Genre")]
    [ForeignKey("GenreId")]
    [InverseProperty("Movies")]
    public virtual Genre Genre { get; set; } = null!;

    [InverseProperty("Movie")]
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
}
