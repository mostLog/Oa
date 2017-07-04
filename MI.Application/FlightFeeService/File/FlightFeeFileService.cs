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
    public class FlightFeeFileService : EPPlusExcelExportBase, IFlightFeeFileService
    {
        //租房表仓储
        private readonly IBaseRepository<FlightFee> _repository;
        //字典表服务
        private readonly ISTypeService _stypeService;
        public FlightFeeFileService(
            IBaseRepository<FlightFee> repository,
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
        public ExcelDto ExcelExport(FlightFeePagedInputDto input)
        {
            var locations = _stypeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            var list = _repository.GetAll();
            list = list
                .AsNoTracking()
                .OrderBy(m => m.f_operator)
                .WhereIf(!input.Name.IsNullOrEmpty(), m => m.t_employeeInfo.f_chineseName == input.Name)
                .WhereIf(input.FlightStartDate != null, m => m.f_FlightDate!=null&& m.f_FlightDate >= input.FlightStartDate)
                .WhereIf(input.FlightEndDate != null, m => m.f_FlightDate!=null&& m.f_FlightDate <= input.FlightEndDate)
                .WhereIf(input.IsPay != 2, m => m.f_IsPay == (input.IsPay == 1 ? true : false));

            var flightList = list
                .ToList()
                .Select(m =>
                {
                    return new FlightFeeListDto
                    {
                        Id = m.f_ID,
                        EmployeeName = m.t_employeeInfo?.f_chineseName ?? string.Empty,
                        Amount = m.f_Amount,
                        FlightDate = m.f_FlightDate,
                        Flight = m.f_Flight,
                        StartEndAdd = m.f_StartEndAdd,
                        IsPay = m.f_IsPay,
                        Location = locations.FirstOrDefault(c => c.f_tID == m.f_AddressId)?.f_value ?? string.Empty,
                        Operator = m.f_operator,
                        OperatorTime = m.f_operatorTime,
                        Remark = m.f_Remark
                    };
                }).ToList();
            ;
            return ExportExcel(flightList);
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        protected ExcelDto ExportExcel(IList<FlightFeeListDto> items)
        {
            string fileName = "员工机票差额" + DateTime.Now.ToFileTime() + ".xlsx";
            return CreateExcel(fileName, excel =>
            {
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("sheet1");

                AddHeader(
                    sheet,
                    "序号",
                    "员工",
                    "机票差额",
                    "单位",
                    "航班日期",
                    "航班",
                    "起止地点",
                    "缴费状态",
                    "收款地点",
                    "备注"
                );
                AddDatas(sheet, 2, 10, items);

                //列自适应
                for (int i = 1; i <= 10; i++)
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
        protected void AddDatas(ExcelWorksheet sheet, int startRowIndex, int columnLen, IList<FlightFeeListDto> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                FlightFeeListDto dto = items[i];
                sheet.Cells[i + startRowIndex, 1].Value = (i + 1).ToString();
                sheet.Cells[i + startRowIndex, 2].Value = dto.EmployeeName;
                sheet.Cells[i + startRowIndex, 3].Value = dto.Amount;
                sheet.Cells[i + startRowIndex, 4].Value = "₱";
                sheet.Cells[i + startRowIndex, 5].Value = dto.FlightDate!=null&&dto.FlightDate!=DateTime.MinValue?dto.FlightDate.Value.ToString("yyyy-MM-dd"):string.Empty;
                sheet.Cells[i + startRowIndex, 6].Value = dto.Flight;
                sheet.Cells[i + startRowIndex, 7].Value = dto.StartEndAdd;
                sheet.Cells[i + startRowIndex, 8].Value = dto.IsPay ? "√" : "×";
                sheet.Cells[i + startRowIndex, 9].Value = dto.Location;
                sheet.Cells[i + startRowIndex, 10].Value = dto.Remark;
            }
        }
    }
}
