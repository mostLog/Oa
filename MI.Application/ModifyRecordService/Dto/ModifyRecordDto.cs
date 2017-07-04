using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.Dto
{
    public class ModifyRecordDto
    {
        /// <summary>
        /// 自增id
        /// </summary>
        public int f_Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime f_ChangeTime { get; set; }
        /// <summary>
        /// 操作方式（是新增、修改还是删除）
        /// </summary>
        public int f_ActionStatus { get; set; }
        /// <summary>
        /// 操作项目
        /// </summary>
        public int f_ItemCategory { get; set; }
        /// <summary>
        /// 操作者
        /// </summary>
        public string f_ByWho { get; set; }
        /// <summary>
        /// 操作者IP
        /// </summary>
        public string f_ByIP { get; set; }
        /// <summary>
        /// 旧数据
        /// </summary>
        public string f_OldData { get; set; }
        /// <summary>
        /// 新数据
        /// </summary>
        public string f_NewData { get; set; }
        /// <summary>
        /// 修改表的名字
        /// </summary>
        public string f_TableName { get; set; }
        /// <summary>
        /// 是否恢复
        /// </summary>
        public Nullable<bool> f_IsRecovered { get; set; }
    }
}
