using AutoMapper;
using MI.Application.DataExport.Excel;
using MI.Application.Dto;
using MI.Core;
using MI.Core.Domain;
using MI.Core.Extension;
using MI.Data;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MI.Application.File
{
    public class GrantFileService : EPPlusExcelExportBase, IGrantFileService
    {
        //补助表仓储
        private readonly IBaseRepository<Grant> _repository;
        public GrantFileService(
            IBaseRepository<Grant> repository,
            IConfig config) : 
            base(config)
        {
            _repository = repository;
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ExcelDto ExcelExport(GrantPagedInputDto input)
        {

            var grantList = _repository.GetAll();
            grantList = grantList
                .AsNoTracking()
                .WhereIf(!input.Name.IsNullOrEmpty(), m => m.t_employeeInfo != null && (m.t_employeeInfo.f_chineseName.Contains(input.Name.Trim())) || m.t_employeeInfo.f_nickname.Contains(input.Name.Trim()) || m.t_employeeInfo.f_passportName.Contains(input.Name.Trim()))
                .WhereIf(input.IsPayment!=2,m => m.f_IsPayment == (input.IsPayment==1?true:false))
                .WhereIf(input.GrantStartDate != null, m => m.f_GrantDate!=null&& m.f_GrantDate >= input.GrantStartDate)
                .WhereIf(input.GrantEndDate != null,m=> m.f_GrantDate!=null&& m.f_GrantDate <= input.GrantEndDate)
                ;

            var list = Mapper.Map<IList<GrantDto>>(grantList.ToList());
            return ExportExcel(list);
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        protected ExcelDto ExportExcel(IList<GrantDto> items)
        {
            string fileName = "员工外租补助" + DateTime.Now.ToFileTime() + ".xlsx";
            return CreateExcel(fileName, excel =>
            {
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("sheet1");

                AddHeader(
                    sheet,
                    "序号",
                    "员工",
                    "补助金额",
                    "补助月份",
                    "是否发放补助",
                    "发放人"
                );
                AddDatas(sheet, 2,6, items);

                //列自适应
                for (int i = 1; i <= 6; i++)
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
        protected void AddDatas(ExcelWorksheet sheet, int startRowIndex,int columnLen, IList<GrantDto> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                GrantDto dto = items[i];
                sheet.Cells[i + startRowIndex, 1].Value = (i+1).ToString();
                sheet.Cells[i + startRowIndex, 2].Value = dto.t_employeeInfo?.f_chineseName;
                sheet.Cells[i + startRowIndex, 3].Value = dto.f_amount;
                sheet.Cells[i + startRowIndex, 4].Value = dto.f_GrantDate!=DateTime.MinValue?dto.f_GrantDate.ToString("yyyy-MM"):string.Empty;
                sheet.Cells[i + startRowIndex, 5].Value = dto.f_IsPayment? "√" : "×";
                sheet.Cells[i + startRowIndex, 6].Value = dto.f_Payee;
            }
        }
    }
}
