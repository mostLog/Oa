using MI.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MI.Core.Common
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 检查model正确。
        /// </summary>
        /// <param name="model"></param>
        /// <param name="bType">false=个人资料。true=员工管理。是否需要验证特殊字段 </param>
        /// <returns></returns>
        public static bool CheckModel(this Employee model, out eTipsEnum o_eTipsEnum, bool bType = true)
        {
            o_eTipsEnum = eTipsEnum.SuccessfulOperation;
            Regex reg = new Regex(@"^[\u4E00-\u9FA5]*$");
            if (string.IsNullOrWhiteSpace(model.f_chineseName) || !reg.IsMatch(model.f_chineseName))
            {
                //验证中文名字
                o_eTipsEnum = eTipsEnum.ChineseNameIsNullOrEmpty;
            }
            else if (string.IsNullOrWhiteSpace(model.f_nickname))
            {
                //验证昵称
                o_eTipsEnum = eTipsEnum.NickNameIsNullOrEmpty;
            }
            else if (model.f_chineseName.Length > 10)
            {

            }
            else if (model.f_department_tID < 1)
            {
                if (bType)
                {
                    //验证部门
                    o_eTipsEnum = eTipsEnum.DepartmentIsNullOrEmpty;
                }
            }
            else if (string.IsNullOrWhiteSpace(model.f_international))
            {
                //验证国籍
                o_eTipsEnum = eTipsEnum.InternationalIsNullOrEmpty;
            }
            else if (string.IsNullOrWhiteSpace(model.f_passportName))
            {
                //验证护照名字
                o_eTipsEnum = eTipsEnum.PassportNameIsNullOrEmpty;
            }
            else if (model.f_workStatus_tID != null && model.f_workStatus_tID.Value < 1)
            {
                if (bType)
                {
                    //验证工作状态
                    o_eTipsEnum = eTipsEnum.WorkStatusIsNullOrEmpty;
                }
            }
            else if (model.f_IsNewEmp == false && string.IsNullOrWhiteSpace(model.f_emergencyContactNumber))
            {
                //不为新人就检查 紧急联系方式
                o_eTipsEnum = eTipsEnum.EmergencyContactNumberIsNull;
            }
            else if (model.f_IsNewEmp == false && string.IsNullOrWhiteSpace(model.f_emergencyContactPerson))
            {
                //不为新人就检查 紧急联系人
                o_eTipsEnum = eTipsEnum.EmergencyContactPersonIsNull;
            }
            else if (model.f_IsNewEmp == false && (model.f_workLocation_tID == null || model.f_workLocation_tID < 1))
            {
                //上班地点  不检查新人的报到地点
                o_eTipsEnum = eTipsEnum.WorkLocationIsNull;
            }
            else if (model.f_level_tID < 1)
            {
                //验证等级权限
                o_eTipsEnum = eTipsEnum.LevelIsNullOrEmpty;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_passportName) && model.f_passportName.Length > 50)
            {
                //验证护照名是否超出长度
                o_eTipsEnum = eTipsEnum.PassportNameLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_passportID) && model.f_passportID.Length > 15)
            {
                //验证护照ID是否超出长度
                o_eTipsEnum = eTipsEnum.PassportIdLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_nickname) && model.f_nickname.Length > 20)
            {
                //验证qq昵称是否超出长度
                o_eTipsEnum = eTipsEnum.NicknameLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_QQNumber) && model.f_QQNumber.Length > 15)
            {
                //验证qq号码是否超出长度
                o_eTipsEnum = eTipsEnum.QqNumberLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_TelNoCN) && model.f_TelNoCN.Length > 20)
            {
                //验证国内号码是否超出长度
                o_eTipsEnum = eTipsEnum.TelNoCnLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_TelNoPH) && model.f_TelNoPH.Length > 20)
            {
                //验证菲线号码是否超出长度
                o_eTipsEnum = eTipsEnum.TelNoPhLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_emergencyContactPerson) && model.f_emergencyContactPerson.Length > 10)
            {
                //验证紧急联系人是否超出长度
                o_eTipsEnum = eTipsEnum.EmergencyContactPersonLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_emergencyContactNumber) && model.f_emergencyContactNumber.Length > 20)
            {
                //验证紧急联系号码是否超出长度
                o_eTipsEnum = eTipsEnum.EmergencyContactNumberLengthExceed;
            }
            else if (!string.IsNullOrWhiteSpace(model.f_Remark) && model.f_Remark.Length > 500)
            {
                //验证备注是否超出长度
                o_eTipsEnum = eTipsEnum.RemarkLengthExceed;
            }
            else if (model.f_IsNewEmp)
            {
                //如果有新人数据  检查新人
                model.NewEmployee.checkModel(out o_eTipsEnum);
            }
            return (o_eTipsEnum == eTipsEnum.SuccessfulOperation);
        }

        /// <summary>
        /// 新人检查
        /// </summary>
        /// <param name="model"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool checkModel(this NewEmployee model, out eTipsEnum e)
        {
            e = eTipsEnum.SuccessfulOperation;

            return (e == eTipsEnum.SuccessfulOperation);
        }

    }
}
