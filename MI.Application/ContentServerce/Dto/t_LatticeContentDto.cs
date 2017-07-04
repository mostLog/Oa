using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Application.ContentServerce.Dto
{
    public class t_LatticeContentDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public int f_LId { get; set; }
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public Nullable<int> f_tID { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        public Nullable<int> f_floor { get; set; }
        /// <summary>
        /// 房间
        /// </summary>
        public int f_room { get; set; }
        /// <summary>
        /// 宿舍表id
        /// </summary>
        public Nullable<int> f_dormitoryId { get; set; }
        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool f_isUnlock { get; set; }
        /// <summary>
        /// 社区ID
        /// </summary>
        public Nullable<int> f_tID2 { get; set; }
        /// <summary>
        /// 设置可以住多少人
        /// </summary>
        public int f_liveFewPeople { get; set; }
        /// <summary>
        /// 现在已经住了多少人
        /// </summary>
        public int f_sumPeople { get; set; }
        /// <summary>
        /// 是否有绑定宿舍(过期舍弃)
        ///  用不到了，因为房间解锁的时候就要绑定宿舍
        /// /// </summary>
        public bool getIsBinDormitory => f_dormitoryId != null;
        /// <summary>
        /// 人员表泛型集合
        /// </summary>
        public ICollection<Employee> listEmployee { get; set; }
        /// <summary>
        /// 住的人
        /// </summary>
        public string[] getArrName
        {
            get
            {
                if (listEmployee != null && listEmployee.Any())
                {
                    return listEmployee.Select(p => p.f_chineseName + "&" + p.f_IsNewEmp + "&" + p.f_eid.ToString()).ToArray();
                }
                return new string[] { "暂无" };
            }
        }
        /// <summary>
        /// 房型
        /// </summary>
        public string f_roomType { get; set; }
        /// <summary>
        /// 房号
        /// </summary>
        public string f_roomNo { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int f_department_tID { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }

        /// <summary>
        /// 获取状态类名，显示不同颜色
        /// </summary>
        public string getClassName
        {
            get
            {
                string sClassName = !f_isUnlock ? "RoomLocking" : "";
                sClassName += sClassName == "" && !getIsBinDormitory ? "noBinDormitory" : "";//是否有绑定到我们的宿舍表ID  用不到了，因为解锁的时候就要绑定
                sClassName += sClassName == "" && f_sumPeople == 0 ? "btn-success" : "";  // 表示已经绑定宿舍表ID 但是还没有住人
                sClassName += sClassName == "" && f_sumPeople < f_liveFewPeople ? "btn-info" : "";  // 表示已经绑定宿舍表ID 但是没住满
                sClassName += sClassName == "" && f_sumPeople == f_liveFewPeople ? "btn-danger" : "";  // 表示已经绑定宿舍表ID 已经刚刚住满
                sClassName += sClassName == "" && f_sumPeople > f_liveFewPeople ? "btn-warning" : "";  // 表示已经绑定宿舍表ID 已经住满 溢出
                return sClassName;
            }
        }
        /// <summary>
        /// 获取住的人的名单
        /// </summary>
        public string getNameHtml
        {
            get
            {
                /*下标0开始控制名字显示 第一次2个 之后3个*/
                int indexName = 1;
                string sHtml = string.Empty;
                for (int i = 0; i < (getArrName.Length > 8 ? 8 : getArrName.Length); i++)
                {
                    if (getArrName[i] == "暂无")
                    {
                        sHtml += "<u>暂无</u>&nbsp;";
                    }
                    else
                    {
                        sHtml += "<u> <a href=\"javascript:goToEmployeeInfo('" + getArrName[i] + "')\">" + getArrName[i].Split('&')[0] + "</a> </u>&nbsp;";
                    }
                    if (indexName == i)
                    {
                        if (indexName == 7 && getArrName.Length > 8)
                        {
                            sHtml += "...";
                        }
                        else
                        {
                            sHtml += "<br />";
                        }
                        indexName = indexName + 3;
                    }
                }
                return sHtml;
            }
        }
        /// <summary>
        /// 双人床数量(填写的文本框)
        /// </summary>
        public int f_DoublesBed { get; set; }
        /// <summary>
        /// 双人床数量(优先,识别房型的)
        /// </summary>
        public int SumDoublesBed
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(f_roomType) && f_roomType.ToUpper().Contains("Q"))
                {
                    //优先识别房型中的几Q 表示几个双人床
                    string temp = f_roomType.Substring(f_roomType.IndexOf("Q") - 1, 1);
                    int sum = 0;
                    int.TryParse(temp, out sum);
                    if (sum != 0)
                    {
                        return sum;
                    }
                }
                return f_DoublesBed;
            }
        }

        public virtual Employee employee { get; set; }
    }
}
