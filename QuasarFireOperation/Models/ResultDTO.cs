using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Models
{
    public class ResultDTO
    {        
        public bool error { get; set; }
        public ResponseDTO response { get; set; }
        public int statusResponse { get; set; }
    }
}
