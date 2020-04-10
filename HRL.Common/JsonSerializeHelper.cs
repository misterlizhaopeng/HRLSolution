using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRL.Common
{
    /// <summary> 
    /// 序列化和反序列化的帮助类
    /// </summary>
    public static class JsonSerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">要序列化的实体</param>
        /// <returns>序列化后的字符串</returns>
        public static string SerializeToJson(object obj)
        {
            //Json.Net
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Object DeserializeFromJson(string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
