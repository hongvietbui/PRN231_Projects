using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Entities
{
    public partial class Country
    {
        public Country()
        {
            Films = new HashSet<Film>();
        }

        [Key]
        [StringLength(3)]
        public string CountryCode { get; set; } = null!;
        [StringLength(50)]
        public string? CountryName { get; set; }

        [InverseProperty("CountryCodeNavigation")]
        public virtual ICollection<Film> Films { get; set; }
    }
}
