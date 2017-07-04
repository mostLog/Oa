using MI.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application
{
    public class ResultModel
    {
        public ResultModel(ErrorEnum eResult, eTipsEnum eTips)
        {
            result = (int)eResult;
            tips = eTips.Description();
        }
        public ResultModel(ErrorEnum eResult, eTipsEnum eTips, string sEexceptionMessage)
        {
            result = (int)eResult;
            tips = eTips.Description();
            exceptionMessage = sEexceptionMessage;
        }
        public ResultModel(int iResult, string sTips)
        {
            result = iResult;
            tips = sTips;
        }
        public ResultModel(int iResult, string sTips, string sEexceptionMessage)
        {
            result = iResult;
            tips = sTips;
            exceptionMessage = sEexceptionMessage;
        }

        public ResultModel(int iResult)
        {
            result = iResult;
        }
        /// <summary>
        /// 结果 是否成功
        /// </summary>
        public int result { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string tips { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string exceptionMessage { get; set; } = "";
    }
}
