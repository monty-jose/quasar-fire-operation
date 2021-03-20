using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Models
{
    public class MessageDTO
    {
        public int id { get; set; }
        
        public int satelite_id { get; set; }

        public double distances { get; set; }

        public string message_array { get; set; }

        public PositionDTO position { get; set; }
    }
}
