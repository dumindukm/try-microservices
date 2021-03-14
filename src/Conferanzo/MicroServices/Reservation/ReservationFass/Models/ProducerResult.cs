using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Text;



namespace ReservationFass.Models
{
    public class ProducerResult
    {
        public bool Valid { get; set; } = true;
        public EventData Message { get; set; } = null;
    }
}
