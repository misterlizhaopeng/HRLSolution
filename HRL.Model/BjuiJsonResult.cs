using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model
{
    public class BjuiJsonResult
    {
        public bool closeCurrent { get; set; }
        public string forward { get; set; }
        public string forwardConfirm { get; set; }
        public int ID { get; set; }
        public string message { get; set; }
        public string statusCode { get; set; }
        public string tabid { get; set; }
    }
}
