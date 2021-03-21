using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuasarFireOperation.Entities
{
    [Table("STATUS_MESSAGES")]
    public class StatusMessages
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}
