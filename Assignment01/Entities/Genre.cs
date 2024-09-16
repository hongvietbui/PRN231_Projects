using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Entities
{
    public partial class Genre
    {
        public Genre()
        {
            Films = new HashSet<Film>();
        }

        [Key]
        public int GenreID { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }

        [InverseProperty("Genre")]
        public virtual ICollection<Film> Films { get; set; }
    }
}
