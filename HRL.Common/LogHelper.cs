using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace HRL.Common
{
    public class LogHelper
    {
        public static bool WriteLog(Exception ex,string message)
        {
            ILog log = log4net.LogManager.GetLogger(typeof(LogHelper).FullName);
            log.Error(message, ex);
            return true;
        }
    }
}
