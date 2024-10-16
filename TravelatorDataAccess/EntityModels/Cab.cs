using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorDataAccess.EntityModels
{
    public class Cab
    {
        public Guid Id { get; set; }
        public required string DriverName { get; set; } 
        public required string ContactNumber { get; set; }
        public required string VehicleName { get; set; }
        public required string VehicleNumber { get; set; }
        public int Capacity { get; set; }

    }
}
