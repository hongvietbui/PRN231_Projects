using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Entities
{
    public partial class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public int ShowID { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        [StringLength(1000)]
        public string? SeatStatus { get; set; }
        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        [ForeignKey("ShowID")]
        [InverseProperty("Bookings")]
        public virtual Show Show { get; set; } = null!;
    }
}
