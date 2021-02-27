using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiGateway
{
    public class ServiceMeta
    {
        public Meta Conference { get; set; }
    }

    public class Meta
    {
        public string Url { get; set; }
        public string AuthCode { get; set; }
    }
}
