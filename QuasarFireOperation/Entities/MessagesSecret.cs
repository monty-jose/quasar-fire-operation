using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuasarFireOperation.Entities
{
    [Table("MESSAGES_SECRET")]
    public class MessagesSecret
    {
        public int id { get; set; }

        public int satelite_id { get; set; }

        public double distance { get; set; }

        public string message { get; set; }
        
        public int? response_id { get; set; }

        public DateTime date_process { get; set; }

        public decimal process { get; set; }

        
    }
}
