using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorService.DTO_s
{
    public class ExpenseDTO
    {
        public Guid RequestId { get; set; }

        public string ExpenseType { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public string ReceiptUrl { get; set; }= string.Empty;
    }
}
