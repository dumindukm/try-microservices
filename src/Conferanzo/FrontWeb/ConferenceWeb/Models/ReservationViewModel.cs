using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceWeb.Models
{
    public class ReservationViewModel
    {
        public string ConferenceCode { get; set; }
        public string ConferenceName { get; set; }
        public int ReserveSeats { get; set; }
        public decimal SeatPrice { get; set; }
    }
}
