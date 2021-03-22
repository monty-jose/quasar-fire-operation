using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace QuasarFireOperation.Entities
{
    [Table("RESPONSE")]
    public class ResponseEntity
    {
        public int id { get; set; }

        public double x_location { get; set; }

        public double y_location { get; set; }

        public string message { get; set; }
        public int status_id { get; set; }
    }
}
