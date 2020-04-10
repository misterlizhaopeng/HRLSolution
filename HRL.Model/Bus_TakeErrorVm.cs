using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model
{
    [MetadataType(typeof(Bus_TakeErrorMetadata))]
    public partial class Bus_TakeError: Pager
    {
        public List<string> AttributeIds { get; set; }
        public List<Bus_TakeErrorAttribute> AttrList { get; set; }

        public string FromSourceName { get; set; }
        public string TypeValName { get; set; }
        public string OverQualityName { get; set; }

        public Nullable<System.DateTime> BeginCreateTime { get; set; }
        public Nullable<System.DateTime> EndCreateTime { get; set; }
    }

    class Bus_TakeErrorMetadata
    {

    }

}
