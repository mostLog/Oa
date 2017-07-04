using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Extension
{
    public static class ConvertExtension
    {
        /// <summary>
        /// 值类型转换 (T为string,char,bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="sourceValue">源值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T ConvertValue<T>(this object sourceValue, T defaultValue)
        {
            //ToString//ToChar                          字符串
            //ToBoolean                                 布尔值
            //ToDateTime                                时间
            //ToDouble//ToDecimal//ToSingle             小数
            //ToUInt64//ToUInt32//ToUInt16//ToByte      无符号数
            //ToInt64//ToInt32//ToInt32//ToSByte        有符号数
            //ToBase64String//ToBase64CharArray         特殊(不需要)
            object getValue = defaultValue;
            Type t = typeof(T);
            if (sourceValue != null)
            {
                getValue = sourceValue;
                if (t == typeof(string) || t == typeof(char))
                {
                    if (getValue.ToString() == string.Empty)
                    {
                        getValue = defaultValue;
                    }
                }
                if (t == typeof(bool))
                {
                    bool temp;
                    if (bool.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                if (t == typeof(DateTime))
                {
                    DateTime temp;
                    if (DateTime.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  小数(可为负数)
                if (t == typeof(double) || t == typeof(decimal) || t == typeof(float))
                {
                    double temp;
                    if (double.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  无符号整数(不可为负数)
                if (t == typeof(ulong) || t == typeof(uint) || t == typeof(ushort) || t == typeof(byte))
                {
                    ulong temp;
                    if (ulong.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  有符号整数(可为负数)
                if (t == typeof(long) || t == typeof(int) || t == typeof(short) || t == typeof(sbyte))
                {
                    long temp;
                    if (long.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
            }
            return (T)Convert.ChangeType(getValue, t);
        }

        /// <summary>
        /// 可为空的值类型转换 (T只能为bool,DateTime,double,decimal,float,ulong,uint,ushort,byte,long,int,short,sbyte)
        /// (string,char不需要使用Nullable类型)
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="sourceValue">源值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T? ConvertValue<T>(this object sourceValue, T? defaultValue) where T : struct
        {
            //ToString//ToChar                          字符串(不需要Nullable转换)
            //ToBoolean                                 布尔值
            //ToDateTime                                时间
            //ToDouble//ToDecimal//ToSingle             小数
            //ToUInt64//ToUInt32//ToUInt16//ToByte      无符号数
            //ToInt64//ToInt32//ToInt32//ToSByte        有符号数
            //ToBase64String//ToBase64CharArray         特殊(不需要)
            object getValue = defaultValue.HasValue ? defaultValue.Value : sourceValue;
            Type t = typeof(T);
            if (sourceValue != null)
            {
                getValue = sourceValue;
                if (t == typeof(bool))
                {
                    bool temp;
                    if (bool.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                if (t == typeof(DateTime))
                {
                    DateTime temp;
                    if (DateTime.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  小数(可为负数)
                if (t == typeof(double) || t == typeof(decimal) || t == typeof(float))
                {
                    double temp;
                    if (double.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  无符号整数(不可为负数)
                if (t == typeof(ulong) || t == typeof(uint) || t == typeof(ushort) || t == typeof(byte))
                {
                    ulong temp;
                    if (ulong.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
                //  无符号整数(可为负数)
                if (t == typeof(long) || t == typeof(int) || t == typeof(short) || t == typeof(sbyte))
                {
                    long temp;
                    if (long.TryParse(getValue.ToString(), out temp))
                    {
                        getValue = temp;
                    }
                    else
                    {
                        getValue = defaultValue;
                    }
                }
            }
            if (getValue == null)
            {
                return (T?)getValue;
            }
            return (T?)Convert.ChangeType(getValue, t);
        }
    }
}
