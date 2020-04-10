using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace HRL.Common.Utlity
{
    public class EncryptHelper
    {
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(string encryptString)
        {
            TripleDESEncryptor target = new TripleDESEncryptor("nihao");

            return target.Encrypt(encryptString);

            //string deCodeString = target.Decrypt(encodeString);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="deEncryptString">要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DeEncryptString(string deEncryptString)
        {
            TripleDESEncryptor target = new TripleDESEncryptor("nihao");
            return target.Decrypt(deEncryptString);
        }

        public static string GetMd5(string txt)
        {
            MD5Encryptor target = new MD5Encryptor(); // TODO: 初始化为适当的值
            return target.GetMD5(txt);
        }
        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>返回加密之后的值</returns>
        public static string EncryptPwd(string pwd)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(FormsAuthentication.HashPasswordForStoringInConfigFile(pwd.ToUpper().ToString(), "MD5").ToUpper(), "MD5");
        }

    }
}
