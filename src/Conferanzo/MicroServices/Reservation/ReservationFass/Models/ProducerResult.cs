using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Text;



namespace ReservationFass.Models
{
    public class ProducerResult
    {
        public bool Valid { get; set; } = true;
        public EventGridEvent Message { get; set; } = null;
    }
}
