using HRL.Common;
using HRL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Data.Entity;
namespace HRL.BLL
{
    /// <summary>
    /// 访问不同语言资源的入口
    /// </summary>
    public class GlobalLngResource
    {
        private readonly static Sys_MultiLangInfoService lngSysRes = new Sys_MultiLangInfoService();

        /// <summary>
        /// 从资源文件中读取要 “打印”，“导出”的指定字段信息，这些字段信息是排好顺序的
        /// </summary>
        /// <param name="notfield">不读取的字段数组</param>
        /// <param name="vModel">表名</param>
        /// <returns></returns>
        public static string[] GetColumnsFrmRes(string[] notfield, string vModel = "")
        {
            string lan = "zh-cn";// HttpContext.Current.Request.Cookies["language"].Value;
            if (string.IsNullOrWhiteSpace(vModel))
            {
                vModel = "PUBLIC";
            }
            var res = lngSysRes.LoadEntities(s => s.COLUMN_CULTURE == lan && s.TABLE_NAME == vModel.ToUpper());
            if (res == null)
            {
                return null;
            }
            res = res.Where(s => !notfield.Contains(s.COLUMN_ID));
            return res.OrderBy(s => s.ORDERBY).Select(s => s.COLUMN_ID).ToArray();
        }

        public static string GetRes(string keyVal, string vModel = "")
        {
            string lan = "zh-cn";//   string lan = HttpContext.Current.Request.Cookies["language"].Value;
            if (string.IsNullOrWhiteSpace(vModel))
            {
                vModel = "public";
            }
            //所有指定资源，此处适当的时候可以加个缓存以防止访问"多语言项"就去请求一次数据库
            List<Sys_MultiLangInfo> allLangData = CacheHelper.GetCache("MultiLangKey") as List<Sys_MultiLangInfo>;
            var cacheLang = CacheHelper.GetCache("lang") as string;
            if (cacheLang != lan)
            {
                allLangData = null;
            }
            if (allLangData == null)
            {
                var allRes = lngSysRes.LoadEntities(s => s.COLUMN_CULTURE == lan).AsNoTracking().ToList();
                CacheHelper.SetCache("MultiLangKey", allRes, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
                CacheHelper.SetCache("lang", lan);
                allLangData = CacheHelper.GetCache("MultiLangKey") as List<Sys_MultiLangInfo>;
            }
            var desc = allLangData.Where(s => s.TABLE_NAME == vModel  && s.COLUMN_ID == keyVal).Select(s => s.COLUMN_NAME).FirstOrDefault();
            return desc;
        }
        /// <summary>
        /// 更新最新缓存
        /// </summary>
        public static void SetMultiLangKey()
        {
            string lan = "zh-cn";//     string lan = HttpContext.Current.Request.Cookies["language"].Value;
            var allRes = lngSysRes.LoadEntities(s => s.COLUMN_CULTURE == lan).AsNoTracking().ToList();
            CacheHelper.SetCache("MultiLangKey", allRes, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1));
        }
    }
}
