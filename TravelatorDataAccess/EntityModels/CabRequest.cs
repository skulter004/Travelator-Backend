using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class CabRequest
    {
        public Guid Id { get; set; }
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }
        public required string PickUp {  get; set; }
        public required string DropOff { get; set; }
        public DateTime Time {  get; set; }
        public bool MonthlyRequest { get; set; }
        public bool RequestApproved { get; set; }
        public Employee Employee { get; set; }
    }
}
