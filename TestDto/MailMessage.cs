using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDto
{
    public class MailMessage
    {
        public string Sender { get; set; }
        public string Reply { get; set; }
        public string Message { get; set; }

        public DateTime RevoveryTime { get; set; }
    }
}
