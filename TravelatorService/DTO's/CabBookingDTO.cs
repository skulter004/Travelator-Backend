﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelatorService.DTO_s
{
    public class CabBookingDTO
    {
        public Guid EmployeeId { get; set; }
        public Guid CabId { get; set; }
        public string PickUp {  get; set; }
        public string DropOff { get; set; }
        public DateTime Time {  get; set; }
        public bool MonthlyRequest {  get; set; }
    }
}
