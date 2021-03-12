using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationFass.Models
{
    public class Reservation
    {
        public string ConferenceCode { get; set; }
        public string ConferenceName { get; set; }
        public int ReserveSeats { get; set; }
        public decimal SeatPrice { get; set; }
    }
}
