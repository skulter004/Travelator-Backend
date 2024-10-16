using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class TravelRequest
    {
        [Key]
        public Guid RequestId { get; set; }

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        public string TravelType { get; set; }  

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Destination { get; set; }

        public string Purpose { get; set; }

        public decimal EstimatedCost { get; set; }

        public string Status { get; set; }  

        public bool BudgetExceeded { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Employee Employee { get; set; }
        public ICollection<Approval> Approvals { get; set; }
        public TripBooking Booking { get; set; }
    }
}
