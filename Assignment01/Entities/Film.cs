using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Entities
{
    public partial class Film
    {
        public Film()
        {
            Shows = new HashSet<Show>();
        }

        [Key]
        public int FilmID { get; set; }
        public int GenreID { get; set; }
        [StringLength(100)]
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        [StringLength(3)]
        public string CountryCode { get; set; } = null!;
        [StringLength(150)]
        public string? FilmUrl { get; set; }

        [ForeignKey("CountryCode")]
        [InverseProperty("Films")]
        public virtual Country CountryCodeNavigation { get; set; } = null!;
        [ForeignKey("GenreID")]
        [InverseProperty("Films")]
        public virtual Genre Genre { get; set; } = null!;
        [InverseProperty("Film")]
        public virtual ICollection<Show> Shows { get; set; }
    }
}
