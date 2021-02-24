using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceWeb.Models
{
    public class ConferenceViewModel
    {
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public int AvilableSeats { get; set; }
        public decimal TicketPrice { get; set; }
    }
}
