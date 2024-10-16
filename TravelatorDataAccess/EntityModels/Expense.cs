using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class Expense
    {
        [Key]
        public Guid ExpenseId { get; set; }

        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        [ForeignKey("TravelRequest")]
        public Guid RequestId { get; set; }

        public string ExpenseType { get; set; } 

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Description {  get; set; }

        public string ReceiptUrl { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Employee Employee { get; set; }
        public TravelRequest TravelRequest { get; set; }
    }
}
