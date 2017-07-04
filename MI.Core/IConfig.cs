using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core
{
    public interface IConfig
    {
        string ConnectionString { get; }
        /// <summary>
        /// 临时文件夹
        /// </summary>
        string TempFoldPath { get; }
    }
}
