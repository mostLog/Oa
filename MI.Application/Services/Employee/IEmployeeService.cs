using MI.Application.Dto;
using MI.Core.Common;
using MI.Core.Domain;
using MI.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MI.Application
{
    [UnitOfWork]
    public interface IEmployeeService
    {
        /// <summary>
        /// 通过id获取员工信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Employee GetEmployeeById(int id);
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Employee Login(LoginDto input);
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<Employee> GetemployeeInfoByWhere(Expression<Func<Employee, bool>> predicate);
        /// <summary>
        /// 无条件分页查询
        /// </summary>
        /// <returns></returns>
        IList<Employee> GetEmployeeByWhere(int pageIndex, int pageSize, out int o_Count, bool bIsNew = false);
        /// <summary>
        /// 条件分页查询
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param> 
        /// <param name="o_Count"></param>
        /// <param name="employeeDto">人员表dto</param>
        /// <returns></returns>
        IList<Employee> GetEmployeeByWhere(EmployeeDto employeedto, int iPageIndex, int iPageSize, out int o_Count, bool bIsNew = false);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<Employee> GetEmployeeByName(string name);
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        List<SType> GetSectorName(Func<SType, bool> predicate);
        /// <summary>
        /// 获取在职状态
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<SType> GetInServiceStatus(Func<SType, bool> predicate);
        /// <summary>
        /// 权限等级
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<SType> GetPrivilegeLevel(Func<SType, bool> predicate);
        /// <summary>
        /// 订餐类型
        /// </summary>
        /// <param name="predicate">条件</param>
        /// <returns></returns>
        List<SType> GetOrderType(Func<SType, bool> predicate);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="feid">员工id</param>
        void DeleteEmployeeInfoById(int feid);
        /// <summary>
        /// 修改员工信息(为了兼容房间、宿舍之类功能中需要修改员工信息而保留的方法)
        /// </summary>
        /// <param name="modelEmployeeInfo"></param>
        /// <param name="bType">是否为个人资料的修改默认否</param>
        void UpdateEmployeeInfo(Employee employeeInfo, bool bType = false);

        void UpdateEmployeeInfo(AllEmployeeInfoViewModel allEmployeeInfoViewModel, bool bType = false, bool isJLKF = false);
       
        List<Employee> SelectByName(string sName);
        /// <summary>
        /// 根据昵称，中文名，护照名查找,OA帐号(模糊匹配)
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
        List<Employee> GetNames(string sName);
        /// <summary>
        /// 根据员工id删除员工
        /// </summary>
        /// <param name="feid"></param>
        /// <returns></returns>
        void DeleteEmoloyeeByfeid(int feid);
        /// <summary>
        /// excel导出全部数据
        /// </summary>
        /// <returns></returns>
        List<dynamic> ExportData();
        /// <summary>
        /// 根据条件导出数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        List<dynamic> ExportData(EmployeeDto employeedto);
        /// <summary>
        /// 获取对应部门新增的帐号名
        /// </summary>
        /// <param name="iDepartmentId">部门id</param>
        /// <returns></returns>
        string GetMaxAccountName(int iDepartmentId);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="modelEmployeeInfo"></param>
        /// <returns></returns>
        void AddEmployeeInfo(AllEmployeeInfoViewModel allEmployeeInfoViewModel);
        /// <summary>
        /// 洗衣房数据
        /// </summary>
        /// <returns></returns>
        List<LaundryPwd> GetLaundryPwdAllData();
        /// <summary>
        /// 移走新人
        /// </summary>
        /// <param name="eid"></param>
        void RemovalEmployeeInfoById(int eid);
        /// <summary>
        /// 获取以移走新人数据
        /// </summary>
        /// <param name="employee"></param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_iCount"></param>
        /// <returns></returns>
        List<Employee> GetNewEmpRemove(EmployeeDto employeedto, int iPageIndex,
          int iPageSize, out int o_iCount);
        /// <summary>
        /// 获取打印以移走新人数据
        /// </summary>
        /// <param name="employeedto"></param>
        /// <returns></returns>
        List<Employee> GetNewEmpRemove(EmployeeDto employeedto);
        /// <summary>
        /// 打印以移走新人数据
        /// </summary>
        /// <param name="employeedto"></param>
        /// <returns></returns>
        List<dynamic> ExportRemoveNewEmp(EmployeeDto employeedto);
        /// <summary>
        /// 获取所有班车数据
        /// </summary>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_Count"></param>
        /// <returns></returns>
        List<Employee> GetShuttleBus(int iPageIndex, int iPageSize, out int o_Count);
        /// <summary>
        /// 根据部门id获取班车数据
        /// </summary>
        /// <param name="f_department_Id">部门id</param>
        /// <param name="iPageIndex"></param>
        /// <param name="iPageSize"></param>
        /// <param name="o_Count"></param>
        /// <returns></returns>
        List<Employee> GetShuttleBus(int f_department_Id, int iPageIndex, int iPageSize, out int o_Count);
        /// <summary>
        /// 班车汇总
        /// </summary>
        /// <param name="sCommunityStr"></param>
        /// <param name="sWorkLocationStr"></param>
        /// <param name="isY">是否包括返乡</param>
        /// <returns></returns>
        List<TrafficStatisticsDto> GetBusSummary(string[] sCommunityStr, string[] sWorkLocationStr, string isY);
        /// <summary>
        /// 人事数据统计表
        /// </summary>
        /// <returns></returns>
        List<Employee> GetPersonnelData();
        /// <summary>
        /// 新人登记打印数据
        /// </summary>
        /// <returns></returns>
        List<Employee> GetPageListexcel(EmployeeDto employeedto, bool bIsNew = false);
        /// <summary>
        ///  修改护照或者入境章图片路径
        /// </summary>
        /// <param name="oMdel">员工信息</param>
        /// <param name="sUrl">存储的url</param>
        /// <param name="sT">护照or入境章</param>
        /// <returns></returns>
        void UpdetePassporUrl(Employee employee, string sUrl, string sT);
        /// <summary>
        /// 获得所有员工信息
        /// </summary>
        /// <returns></returns>
        IList<Employee> GetAllEmployeeData();

        /// <summary>
        /// 获取洗衣房以及密码 
        /// </summary>
        /// <param name="iId">洗衣房ID</param>
        /// <returns></returns>
        string GetLaundryPwd(int iId);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        void EditEmployeePwd(ModifyPwdModel model);
    }
}
