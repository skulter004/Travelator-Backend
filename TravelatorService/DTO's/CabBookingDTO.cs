using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorService.DTO_s
{
    public class CabBookingDTO
    {
        public string PickUp {  get; set; }
        public string DropOff { get; set; }
        public DateTime Time {  get; set; }
        public bool MonthlyRequest {  get; set; }
    }
}
