using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class CabBooking
    {
        public Guid Id { get; set; }
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        public required string PickUp { get; set; }
        public TimeOnly Time {  get; set; }
        public Guid CabId { get; set; }
        public Employee Employee { get; set; }
    }
}
