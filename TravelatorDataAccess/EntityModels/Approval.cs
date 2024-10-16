using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class Approval
    {
        [Key]
        public Guid ApprovalId { get; set; }

        [ForeignKey("TravelRequest")]
        public Guid RequestId { get; set; }

        [ForeignKey("Employee")]
        public Guid ApproverId { get; set; }  
        public string ApprovalLevel { get; set; }  

        public string Status { get; set; } 

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public TravelRequest TravelRequest { get; set; }
        public Employee Approver { get; set; }
    }
}
