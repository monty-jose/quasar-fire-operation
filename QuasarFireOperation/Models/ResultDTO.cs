using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Models
{
    public class ResultDTO
    {
        public bool result { get; set; }
        public string error { get; set; }
        public ResponseDTO response { get; set; }
    }
}
