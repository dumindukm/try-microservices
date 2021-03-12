using System;
using System.Collections.Generic;
using System.Text;

namespace ReservationFass.Models
{
    public class ActivityResult<T>
    {
        public bool Valid { get; set; } = true;
        public T Item { get; set; }
        public string ExceptionMessage { get; set; } = string.Empty;
    }
}
