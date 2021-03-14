using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationFass.Models
{
    public class CommandProducer
    {
        public string MessageId { get; set; }
        public string CommandName { get; set; }
        public string Source { get; set; }
        public bool OperationStatus { get; set; } = false;
        public Reservation ReservationMessage { get; set; }
        public CommandProducer(string messageId , string source )
        {
            this.MessageId = messageId;
            this.Source = source;
        }

    }
}
