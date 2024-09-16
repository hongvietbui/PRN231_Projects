using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Entities
{
    public partial class Show
    {
        public Show()
        {
            Bookings = new HashSet<Booking>();
        }

        [Key]
        public int ShowID { get; set; }
        public int RoomID { get; set; }
        public int FilmID { get; set; }
        [Column(TypeName = "date")]
        public DateTime ShowDate { get; set; }
        [Column(TypeName = "money")]
        public decimal Price { get; set; }
        public bool? Status { get; set; }
        public int Slot { get; set; }

        [ForeignKey("FilmID")]
        [InverseProperty("Shows")]
        public virtual Film Film { get; set; } = null!;
        [ForeignKey("RoomID")]
        [InverseProperty("Shows")]
        public virtual Room Room { get; set; } = null!;
        [InverseProperty("Show")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
