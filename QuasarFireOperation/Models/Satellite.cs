using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Models
{
    public class Satellite
    {
        public string name { get; set; }
        public double distance { get; set; }
        public string [] message { get; set; }
        public Position position { get; set; }
    }
}
