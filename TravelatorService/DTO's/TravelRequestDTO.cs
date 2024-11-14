using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorService.DTO_s
{
    public class TravelRequestDTO
    {
        public Guid EmployeeId { get; set; }
        public string TravelType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Destination { get; set; }

        public string Purpose { get; set; }

        public decimal EstimatedCost { get; set; }

    }
}
