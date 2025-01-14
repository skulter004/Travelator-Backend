using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class Employee
    {
        [Key]
        [Column(TypeName = "char(36)")]
        public Guid EmployeeId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Department { get; set; }

        public decimal BudgetLimit { get; set; }

        public decimal UsedBudget { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        public ICollection<TravelRequest> TravelRequests { get; set; }
        public ICollection<Approval> Approvals { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
