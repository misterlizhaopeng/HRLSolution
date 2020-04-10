using System.Web;
using System.Web.Mvc;

namespace HRL.UI.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //  filters.Add(new MyHandlerMessageAttribute());
        }




    }
    public class MyHandlerMessageAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
                filterContext.Exception.Message == "this is ThrowErrorLogin Action Throw")
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Write("5洗ten No Problem<br/>" +
                    filterContext.Exception.ToString());
            }



            //code 
            // base.OnException(filterContext);
        }
    }
}