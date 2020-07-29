using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esender.Models
{
    public class BulkMailModel
    {
        public List<EmailModel> Email_list { get; set; }
        public string Mail_text { get; set; }
    }
}
