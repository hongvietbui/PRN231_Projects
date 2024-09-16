using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Assignment01.Entities
{
    public partial class Room
    {
        public Room()
        {
            Shows = new HashSet<Show>();
        }

        [Key]
        public int RoomID { get; set; }
        [StringLength(100)]
        public string? Name { get; set; }
        public int? NumberRows { get; set; }
        public int? NumberCols { get; set; }

        [InverseProperty("Room")]
        public virtual ICollection<Show> Shows { get; set; }
    }
}
