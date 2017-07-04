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
    public class TariffFileService : EPPlusExcelExportBase, ITariffFileService
    {
        //仓储
        private readonly IBaseRepository<Tariff> _repository;
        public TariffFileService(
            IBaseRepository<Tariff> repository,
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
        public ExcelDto ExcelExport(TariffPagedInputDto input)
        {

            var tariffList = _repository.GetAll();
            tariffList = tariffList
                .AsNoTracking() //无跟踪状态
                .WhereIf(!string.IsNullOrWhiteSpace(input.Community), m => m.t_dormitory.f_Community.Contains(input.Community))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Building), m => m.t_dormitory.f_Building.Contains(input.Building))
                .WhereIf(!string.IsNullOrWhiteSpace(input.RoomNo), m => m.t_dormitory.f_RoomNo.Contains(input.RoomNo))
                .WhereIf(input.TariffStartDate != null, m =>m.f_TariffDate!=null&& m.f_TariffDate >= input.TariffStartDate.Value)
                .WhereIf(input.TariffEndDate != null, m => m.f_TariffDate != null && m.f_TariffDate <= input.TariffEndDate.Value)
                .WhereIf(input.IsPayment != 2, m => m.f_IsPayment == (input.IsPayment == 1 ? true : false));

            var list = tariffList
                .ToList()
                .Select(m => {
                    return new TariffListDto()
                    {
                        Id = m.f_Id,
                        Community = m.t_dormitory?.f_Community ?? string.Empty,
                        Building = m.t_dormitory?.f_Building ?? string.Empty,
                        RoomNo = m.t_dormitory?.f_RoomNo ?? string.Empty,
                        TariffStandard = m.f_TariffStandard,
                        RealBill = m.f_RealBill,
                        Overruns = m.f_Overruns,
                        TariffDate = m.f_TariffDate,
                        Registrant = m.f_Registrant,
                        IsPayment = m.f_IsPayment,
                        Rate = m.f_Rate,
                        Location = "",
                        Operator = m.f_operator,
                        OperatorTime = m.f_operatorTime,
                        Remark = m.f_Remark
                    };
                }).ToList();
            return ExportExcel(list);
        }
        /// <summary>
        /// Excel导出
        /// </summary>
        protected ExcelDto ExportExcel(IList<TariffListDto> items)
        {
            string fileName = "电费超支" + DateTime.Now.ToFileTime() + ".xlsx";
            return CreateExcel(fileName, excel =>
            {
                ExcelWorksheet sheet = excel.Workbook.Worksheets.Add("sheet1");

                AddHeader(
                    sheet,
                    "序号",
                    "社区",
                    "楼栋",
                    "房间号",
                    "电费标准",
                    "实际账单",
                    "超支金额",
                    "登记日期",
                    "登记人",
                    "是否缴费",
                    "收费人",
                    "收款地点",
                    "最后操作日期",
                    "备注"
                );
                AddDatas(sheet, 2,6, items);

                //列自适应
                for (int i = 1; i <= 14; i++)
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
        protected void AddDatas(ExcelWorksheet sheet, int startRowIndex,int columnLen, IList<TariffListDto> items)
        {
            for (var i = 0; i < items.Count; i++)
            {
                TariffListDto dto = items[i];
                sheet.Cells[i + startRowIndex, 1].Value = (i+1).ToString();
                sheet.Cells[i + startRowIndex, 2].Value = dto.Community;
                sheet.Cells[i + startRowIndex, 3].Value = dto.Building;
                sheet.Cells[i + startRowIndex, 4].Value = dto.RoomNo;
                sheet.Cells[i + startRowIndex, 5].Value = dto.TariffStandard;
                sheet.Cells[i + startRowIndex, 6].Value = dto.RealBill;
                sheet.Cells[i + startRowIndex, 7].Value = dto.Overruns;
                sheet.Cells[i + startRowIndex, 8].Value = dto.TariffDate!=null&&dto.TariffDate!=DateTime.MinValue?dto.TariffDate.Value.ToString("yyyy-MM-dd"):string.Empty;
                sheet.Cells[i + startRowIndex, 9].Value = dto.Registrant;
                sheet.Cells[i + startRowIndex, 10].Value = dto.IsPayment ? "√" : "×";
                sheet.Cells[i + startRowIndex, 11].Value = dto.Rate;
                sheet.Cells[i + startRowIndex, 12].Value = dto.Location;
                sheet.Cells[i + startRowIndex, 13].Value = dto.OperatorTime.Value.ToString("yyyy-MM-dd hh:mm:ss");
                sheet.Cells[i + startRowIndex, 14].Value = dto.Remark;
            }
        }
    }
}
