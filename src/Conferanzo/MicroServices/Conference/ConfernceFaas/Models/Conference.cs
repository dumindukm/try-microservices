using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfernceFaas.Models
{
    public class Conference
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        public string Code { get; set; } = "code_0001";
        public string Venue { get; set; }
        public string DisplayName { get; set; } = "Test conf";
        public int AvilableSeats { get; set; } = 100;
        public decimal TicketPrice { get; set; } = 500;
    }
}
