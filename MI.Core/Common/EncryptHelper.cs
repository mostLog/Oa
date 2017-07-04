using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace MI.Core.Common
{
    public class EncryptHelper
    {
        private static RijndaelManaged rijndaelProvide = new RijndaelManaged();
        private static string _KEY = WebConfigurationManager.AppSettings["_Key"] ?? "MZsadNVKJZHIsdBSMAND^v735sdGBDANDBMM";
        private static string _IV = WebConfigurationManager.AppSettings["_Vi"] ?? "najdn1238^Jnfmn123vcn";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str_Context"></param>
        /// <returns></returns>
        public static string AESEncrypt(string str_Context)
        {
            UTF8Encoding myUtf8 = new UTF8Encoding();
            try
            {
                string retstr = "";
                byte[] input = myUtf8.GetBytes(str_Context);
                //產生加密實體
                ICryptoTransform encryptor = rijndaelProvide.CreateEncryptor(Encoding.UTF8.GetBytes(_KEY.Substring(0, 32)), Encoding.UTF8.GetBytes(_IV.Substring(0, 16)));
                MemoryStream msEncrypt = new MemoryStream();
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                csEncrypt.Write(input, 0, input.Length);
                csEncrypt.FlushFinalBlock();
                foreach (byte s in msEncrypt.ToArray())
                {
                    retstr += s.ToString("X2");    //加密過的後字串採16進制兩位
                }

                return retstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str_Cipher"></param>
        /// <returns></returns>
        public static string AESDecrypt(string str_Cipher)
        {
            UTF8Encoding myUtf8 = new UTF8Encoding();
            try
            {
                byte[] byte_Cipher = new byte[str_Cipher.Length / 2];
                int j = 0;
                for (int i = 0; i < str_Cipher.Length && j < byte_Cipher.Length; i += 2)
                {
                    byte_Cipher[j] = (byte)Convert.ToInt32(str_Cipher.Substring(i, 2), 16);
                    j++;
                }

                //產生解密實體
                ICryptoTransform decryptor = rijndaelProvide.CreateDecryptor(Encoding.UTF8.GetBytes(_KEY.Substring(0, 32)), Encoding.UTF8.GetBytes(_IV.Substring(0, 16)));
                MemoryStream msDecrypt = new MemoryStream(byte_Cipher);
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                byte[] fromEncrypt = new byte[byte_Cipher.Length];
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                return myUtf8.GetString(fromEncrypt).Replace("\0", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

