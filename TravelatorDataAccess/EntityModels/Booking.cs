using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class TripBooking
    {
        [Key]
        public Guid BookingId { get; set; }

        [ForeignKey("TravelRequest")]
        public Guid RequestId { get; set; }

        [ForeignKey("Employee")]
        public Guid AdminId { get; set; } 

        public string BookingDetails { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public TravelRequest TravelRequest { get; set; }
        public Employee Admin { get; set; }
    }
}
