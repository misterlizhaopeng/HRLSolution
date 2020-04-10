using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model
{
    public class Pager
    {
        public string orderDirection { get; set; }
        public string orderField { get; set; }
        public int pageCurrent { get; set; }
        public int pageSize { get; set; }
        public int total { get; set; }

        public Pager()
        {
            pageCurrent = 1;
            pageSize = 20;
        }
    }
}
