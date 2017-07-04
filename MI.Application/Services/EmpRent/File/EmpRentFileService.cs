using MI.Application.DataExport.Excel;
using MI.Application.Dto;
using MI.Core;
using MI.Core.Domain;
using MI.Core.Extension;
using MI.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace MI.Application.File
{
    public class EmpRentFileService : EPPlusExcelExportBase,IEmpRentFileService
    {
        //租房表仓储
        private readonly IBaseRepository<EmpRent> _repository;
        //字典表服务
        private readonly ISTypeService _stypeService;
        public EmpRentFileService(
            IBaseRepository<EmpRent> repository,
            ISTypeService stypeService,
            IConfig config) : 
            base(config)
        {
            _repository = repository;
            _stypeService = stypeService;
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ExcelDto ExcelExport(EmpRentPagedInputDto input)
        {
            //获取上班地点字典数据
            var workLocations = _stypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            var lstEmpRent = _repository.GetAll();
            var empRent = lstEmpRent
                .AsNoTracking()
                //中文名,护照名,昵称
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), m => m.t_employeeInfo != null && (m.t_employeeInfo.f_chineseName.Contains(input.Name.Trim())) || m.t_employeeInfo.f_nickname.Contains(input.Name.Trim()) || m.t_employeeInfo.f_passportName.Contains(input.Name.Trim()))
                //房间号
                .WhereIf(!string.IsNullOrWhiteSpace(input.RoomNo), m => m.t_dormitory != null && m.t_dormitory.f_RoomNo.ToUpper().Contains(input.RoomNo.ToUpper()))
                //是否缴费
                .WhereIf(input.IsPayment != 2, m => m.f_IsPayment == (input.IsPayment == 1 ? true : false))
                //楼栋
                .WhereIf(!string.IsNullOrWhiteSpace(input.Building), m => m.t_dormitory != null && m.t_dormitory.f_Building.ToUpper().Contains(input.Building))
                //缴费日期
                .WhereIf(input.PaymentStartDate != null, m => m.f_PaymentDate != null && m.f_PaymentDate >= input.PaymentStartDate)
                .WhereIf(input.PaymentEndDate!=null,m=>m.f_PaymentDate!=null&&m.f_PaymentDate<=input.PaymentEndDate)
                .ToList()
                .Select(m =>
                {
                    EmpRentListDto dto = new EmpRentListDto();
                    dto.Id = m.f_Id;
                    dto.EmployeeName = m.t_employeeInfo?.f_chineseName ?? string.Empty;
                    dto.DeptName = m.t_employeeInfo?.STypeDepartment?.f_value ?? string.Empty;
                    dto.DormitoryName = m.t_dormitory?.f_Community ?? string.Empty + "/" + m.t_dormitory?.f_Building ?? string.Empty + "/" + m.t_dormitory?.f_RoomNo ?? string.Empty;
                    dto.Rent = m.f_Rent;
                    dto.Grant = m.f_Grant;
                    dto.Amount = m.f_Amount;
                    dto.PaymentDate = m.f_PaymentDate;
                    dto.IsPayment = m.f_IsPayment;
                    dto.Payee = m.f_Payee;
                    dto.Address = workLocations.FirstOrDefault(c => c.f_tID == m.f_AddressId)?.f_value ?? string.Empty;
                    dto.EffectiveDate = m.f_EffectiveDate;
                    dto.Operator = m.f_operator;
                    dto.OperatorTime = m.f_operatorTime;
                    dto.Remark = m.f_Remark;
                    return dto;

                }).ToList();
            ;
            return ExportExcel(empRent);
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        protected ExcelDto ExportExcel(IList<EmpRentListDto> items)
        {
            string fileName = "员工租房信息" + DateTime.Now.ToFileTime() + ".xlsx";
            return CreateExcel(fileName, excel =>
            {
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("sheet1");

                AddHeader(
                    sheet,
                    "序号",
                    "员工",
                    "部门",
                    "宿舍",
                    "租金",
                    "补贴",
                    "应缴费",
                    "缴费月份",
                    "是否缴费",
                    "收款人",
                    "收款地点",
                    "收款时间",
                    "备注"
                );
                AddDatas(sheet, 2,13, items);

                //列自适应
                for (int i = 1; i <= 13; i++)
                {
                    sheet.Column(i).AutoFit();
                }

            });
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="items"></param>
        protected void AddDatas(ExcelWorksheet sheet, int startRowIndex,int columnLen, IList<EmpRentListDto> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                EmpRentListDto dto = items[i];
                sheet.Cells[i + startRowIndex, 1].Value = (i+1).ToString();
                sheet.Cells[i + startRowIndex, 2].Value = dto.EmployeeName;
                sheet.Cells[i + startRowIndex, 3].Value = dto.DeptName;
                sheet.Cells[i + startRowIndex, 4].Value = dto.DormitoryName;
                sheet.Cells[i + startRowIndex, 5].Value = dto.Rent;
                sheet.Cells[i + startRowIndex, 6].Value = dto.Grant;
                sheet.Cells[i + startRowIndex, 7].Value = dto.Amount;
                sheet.Cells[i + startRowIndex, 8].Value = dto.PaymentDate != DateTime.MinValue?dto.PaymentDate?.ToString("yyyy-MM-dd"):string.Empty;
                sheet.Cells[i + startRowIndex, 9].Value = dto.IsPayment ? "√" : "×";
                sheet.Cells[i + startRowIndex, 10].Value = dto.Payee;
                sheet.Cells[i + startRowIndex, 11].Value = dto.Address;
                sheet.Cells[i + startRowIndex, 12].Value = dto.EffectiveDate!=DateTime.MinValue?dto.EffectiveDate?.ToString("yyyy-MM-dd"):string.Empty;
                sheet.Cells[i + startRowIndex, 13].Value = dto.Remark;
            }
        }
    }
}
