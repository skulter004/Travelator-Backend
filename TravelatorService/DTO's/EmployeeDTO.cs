using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorService.DTO_s
{
    public class EmployeeDTO
    {
        public string Name { get; set; }

        public string Department { get; set; }

        public decimal BudgetLimit { get; set; }

        public decimal UsedBudget { get; set; }
        public string Email {  get; set; }
    }
}
