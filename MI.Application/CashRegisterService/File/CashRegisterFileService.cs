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
    public class CashRegisterFileService : EPPlusExcelExportBase, ICashRegisterFileService
    {
        //租房表仓储
        private readonly IBaseRepository<CashRegister> _repository;
        //字典表服务
        private readonly IBaseRepository<Employee> _employeeRepository;
        public CashRegisterFileService(
            IBaseRepository<CashRegister> repository,
            IBaseRepository<Employee> employeeRepository,
            IConfig config) : 
            base(config)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ExcelDto ExcelExport(CashRegisterPagedInputDto input)
        {
            var tempCashRegisterList = _repository
                 .GetAll()
                 .GroupJoin(_employeeRepository.GetAll(), cr => cr.f_Handled_f_Eid, e => e.f_eid, (cr, e) =>
                     new
                     {
                         cr = cr,
                         e = e
                     })
                 .SelectMany(c => c.e.DefaultIfEmpty(), (cr, e) =>
                     new
                     {
                         cr = cr,
                         e = e
                     }
                   )
                   .WhereIf(!string.IsNullOrWhiteSpace(input.Name), c => c.e.f_chineseName.Contains(input.Name.Trim()) || c.e.f_nickname.Contains(input.Name.Trim()) || c.e.f_passportName.Contains(input.Name.Trim()))
                   .WhereIf(input.StarDate != null, c => c.cr.cr.f_Date >= input.StarDate)
                   .WhereIf(input.EndDate != null, c => c.cr.cr.f_Date <= input.EndDate)
                   .WhereIf(input.TypeID != 0, c => c.cr.cr.f_workLocation_f_tid == input.TypeID)
                   .Select(tmp => new CashRegisterListDto
                   {
                       f_CId = tmp.cr.cr.f_CId,
                       f_workLocation_f_tid = tmp.cr.cr.f_workLocation_f_tid,
                       f_Date = tmp.cr.cr.f_Date,
                       f_Items = tmp.cr.cr.f_Items,
                       f_Income = tmp.cr.cr.f_Income,
                       f_Expenses = tmp.cr.cr.f_Expenses,
                       f_Handled_f_Eid = tmp.cr.cr.f_Handled_f_Eid,
                       f_Remark = tmp.cr.cr.f_Remark,
                       f_Balance = tmp.cr.cr.f_Balance,
                       f_HasReceipt = tmp.cr.cr.f_HasReceipt,
                       f_nickname = tmp.e.f_nickname
                   }).ToList();
            ;
            return ExportExcel(tempCashRegisterList);
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        protected ExcelDto ExportExcel(IList<CashRegisterListDto> items)
        {
            string fileName = "现金登记" + DateTime.Now.ToFileTime() + ".xlsx";
            return CreateExcel(fileName, excel =>
            {
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("sheet1");

                AddHeader(
                    sheet,
                    "序号",
                    "日期",
                    "品项",
                    "收入",
                    "支出",
                    "剩余余额",
                    "经手人",
                    "有无收据",
                    "备注"
                );
                AddDatas(sheet, 2,9, items);

                //列自适应
                for (int i = 1; i <= 9; i++)
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
        protected void AddDatas(ExcelWorksheet sheet, int startRowIndex,int columnLen, IList<CashRegisterListDto> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                CashRegisterListDto dto = items[i];
                sheet.Cells[i + startRowIndex, 1].Value = (i+1).ToString();
                sheet.Cells[i + startRowIndex, 2].Value = dto.f_Date!=DateTime.MinValue?dto.f_Date?.ToString("yyyy-MM-dd"):string.Empty;
                sheet.Cells[i + startRowIndex, 3].Value = dto.f_Items;
                sheet.Cells[i + startRowIndex, 4].Value = dto.f_Income;
                sheet.Cells[i + startRowIndex, 5].Value = dto.f_Expenses;
                sheet.Cells[i + startRowIndex, 6].Value = dto.f_Balance;
                sheet.Cells[i + startRowIndex, 7].Value = dto.f_nickname;
                sheet.Cells[i + startRowIndex, 8].Value = dto.f_HasReceipt==true ? "有" : "无";
                sheet.Cells[i + startRowIndex, 9].Value = dto.f_Remark;
               
            }
        }
    }
}
