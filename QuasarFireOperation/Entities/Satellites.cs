using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuasarFireOperation.Entities
{
    [Table("SATELLITES")]
    public class Satellites
    {
        public int id { get; set; }
        public string name { get; set; }
        public double position_x { get; set; }
        public double position_y { get; set; }

    }
}
