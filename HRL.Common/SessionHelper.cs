using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRL.Common
{
    /// <summary>  
    /// Session 操作类  
    /// 1、GetSession(string name)根据session名获取session对象  
    /// 2、SetSession(string name, object val)设置session
    /// 3、RemoveSession(string name) 清除session对象
    /// </summary>  
    public class SessionHelper
    {
        /// <summary>  
        /// 根据session名获取session对象  
        /// </summary>  
        /// <param name="name"></param>  
        /// <returns></returns>  
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }

        /// <summary>  
        /// 设置session  
        /// </summary>  
        /// <param name="name">session 名</param>  
        /// <param name="val">session 值</param>  
        public static void SetSession(string name, object val)
        {
            HttpContext.Current.Session.Remove(name);
            HttpContext.Current.Session.Add(name, val);
            HttpContext.Current.Session.Timeout = 60;
        }

        /// <summary>
        /// 清除session
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveSession(string name)
        {
            HttpContext.Current.Session[name] = null;
            HttpContext.Current.Session.Remove(name);
        }
    }  

}
