using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGateway.Models.Conference
{
    public class Conference
    {
        public string Code { get; set; } //= "code_0001";
        public string DisplayName { get; set; } //= "Test conf";
        public int AvilableSeats { get; set; } //= 100;
        public decimal TicketPrice { get; set; } //= 500;
    }
}
