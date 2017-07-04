using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.ContentServerce.Dto
{
    public class DormitoryDto
    {
        /// <summary>
        /// id
        /// </summary>
        public int f_DormitoryId { get; set; }
        /// <summary>
        /// 宿舍所在社区  
        /// </summary>
        public string f_Community { get; set; }
        /// <summary>
        /// 社区所在楼栋
        /// </summary>
        public string f_Building { get; set; }
        /// <summary>
        /// 宿舍所在房间号
        /// </summary>
        public string f_RoomNo { get; set; }
        /// <summary>
        /// 签约日期
        /// </summary>
        public System.DateTime f_ContractDate { get; set; }
        /// <summary>
        ///租期
        /// </summary>
        public int f_Term { get; set; }
        /// <summary>
        /// 截至日期    
        /// </summary>
        public System.DateTime f_DueDate { get; set; }
        /// <summary>
        /// 中介
        /// </summary>
        public string f_Landlady { get; set; }
        /// <summary>
        /// 租金
        /// </summary>
        public decimal f_Rent { get; set; }
        /// <summary>
        /// 是否有停车位
        /// </summary>
        public bool f_IsBerth { get; set; }
        /// <summary>
        /// 停车位号码
        /// </summary>
        public string f_erthNo { get; set; }
        /// <summary>
        /// 房间类型
        /// </summary>
        public string f_RoomType { get; set; }
        /// <summary>
        /// 洗衣房密码
        /// </summary>
        public string f_LaundryAndPwd { get; set; }
        /// <summary>
        /// 凉衣房密码
        /// </summary>
        public string f_ClothesAndPwd { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string f_Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string f_operator { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public Nullable<System.DateTime> f_operatorTime { get; set; }
        /// <summary>
        /// 限住人数
        /// </summary>
        public Nullable<int> f_totalOfPeople { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public Nullable<int> f_department_tID { get; set; }
        /// <summary>
        /// 床位数量
        /// </summary>
        public Nullable<int> f_DoublesBed { get; set; }

        /// <summary>
        /// 关联外键  宿舍Id
        /// </summary>
        public virtual ICollection<EmpRent> t_EmpRent { get; set; }
        /// <summary>
        /// 人员信息表泛型集合
        /// </summary>       
        public virtual ICollection<Employee> t_employeeInfo { get; set; }
    }
}
