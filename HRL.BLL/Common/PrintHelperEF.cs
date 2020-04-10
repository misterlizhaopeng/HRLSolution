using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRL.BLL 
{
    public class PrintHelperEF
    {
        /// <summary>
        /// 找指定表名下可用的键值对
        /// </summary>
        /// <param name="notfield">排除指定的字段或者列</param>
        /// <param name="outField">返回的字段信息</param>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetDictPrintHeader(string[] notfield, string tableName, out string[] outField)
        {
            string[] field = GlobalLngResource.GetColumnsFrmRes(notfield, tableName);
            outField = field;
            Dictionary<string, string> dictPrintHead = new Dictionary<string, string>();
            foreach (string item in field)
            {
                if (GlobalLngResource.GetRes(item, tableName) != null)
                {
                    dictPrintHead.Add(item, GlobalLngResource.GetRes(item, tableName));
                }
            }
            return dictPrintHead;
        }

        /// <summary>
        /// 构建打印列头
        /// </summary>
        /// <param name="notfield">排除指定的字段或者列</param>
        /// <param name="tableName">表名</param>
        public static string GetPrintLayOut(string[] notfield, string tableName)
        {
            string[] outField;
            Dictionary<string, string> dictPrintHead = GetDictPrintHeader(notfield, tableName, out outField);
            var rtnvalue = "<s:BorderContainer xmlns:fx=\"http://ns.adobe.com/mxml/2009\"  xmlns:s=\"library://ns.adobe.com/flex/spark\" fontFamily=\"宋体\"  xmlns:mx=\"library://ns.adobe.com/flex/mx\" fontSize=\"10\" width=\"100%\" height=\"100%\" minWidth=\"100%\" minHeight=\"100%\">";
            rtnvalue += "<DataGrid x=\"1\" y=\"40\" id=\"dataGrid\" width=\"99%\" height=\"99%\"><columns>";
            foreach (var item in dictPrintHead)
            {
                rtnvalue += "<DataGridColumn visible=\"true\" dataField='" + item.Key + "' headerText='" + item.Value + "' />";
            }
            rtnvalue += "</columns></DataGrid></s:BorderContainer>";
            return rtnvalue;
        }

        /// <summary>
        /// 构建打印数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">泛型数据</param>
        /// <param name="notfield">排除指定的字段或者列</param>
        /// <param name="tableName">表名</param>
        /// <param name="lang">语言</param>
        /// <returns></returns>
        public static string GetPrintData<T>(List<T> list, string[] notfield, string tableName, string lang)
        {
            string[] field = null;
            Dictionary<string, string> dictPrintHead = GetDictPrintHeader(notfield, tableName, out  field);
            var rtnvalue = "<Data><DataGrid><ColModel>";
            foreach (var item in dictPrintHead)
            {
                rtnvalue += "<DataGridColumn DataField='" + item.Key + "' text='" + item.Value + "' />";
            }
            rtnvalue += "</ColModel><RowData>";
            for (int i = 0; i < list.Count; i++)
            {
                rtnvalue += "<Item ";
                //通过反射得到参数对象的属性
                for (int j = 0; j < field.Length; j++)
                {
                    PropertyInfo property = list[i].GetType().GetProperty(field[j].ToString());
                    if (field.Contains(property.Name))
                    {
                        object obj = property.GetValue(list[i], null);
                        if (obj != null)
                        {
                            rtnvalue += field[j].ToString() + "='" + obj.ToString() + "' ";
                        }
                        else
                            rtnvalue += field[j].ToString() + "='' ";
                    }
                }
                rtnvalue += " />";
            }
            System.Web.Routing.RequestContext requestContext = new System.Web.Routing.RequestContext();
            lang = lang == "zh-cn" ? "0" : "1";
            rtnvalue += "</RowData></DataGrid><LanguageType>" + lang + "</LanguageType></Data>";
            return rtnvalue;
        }
    }
}
