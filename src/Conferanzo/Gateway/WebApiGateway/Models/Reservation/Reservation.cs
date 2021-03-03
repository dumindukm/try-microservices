using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGateway.Models.Reservation
{
    public class Reservation
    {
        public string ConferenceCode { get; set; }
        public string ConferenceName { get; set; }
        public int ReserveSeats { get; set; }
        public decimal SeatPrice { get; set; }
    }
}
