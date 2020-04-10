using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRL.Model.Enums
{
    /// <summary>
    /// 获取指定枚举类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EnumDescription<T>
    {
        /// <summary>
        /// 得到指定T类型下面的枚举类中文注释
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string GetVisaTypeEnumDescription(int value)
        { 
            Type type = typeof(T);
            var name = Enum.GetName(typeof(T), value);
            object[] obj = type.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
            DescriptionAttribute attr = obj[0] as DescriptionAttribute;
            return attr.Description;
        }
    }
}
