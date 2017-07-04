using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using MI.Core;
using MI.Application.Dto;

namespace MI.Application.DataExport.Excel
{
    /// <summary>
    /// EPPlus组件导出Excel基类
    /// </summary>
    public abstract class EPPlusExcelExportBase
    {
        protected readonly IConfig _config;
        public EPPlusExcelExportBase(IConfig config)
        {
            _config = config;
        }
        /// <summary>
        /// 创建Excel
        /// </summary>
        /// <param name="fileName">Excel文件名</param>
        /// <param name="action"></param>
        protected ExcelDto CreateExcel(string fileName, Action<ExcelPackage> action)
        {
            ExcelDto file = new ExcelDto(fileName);
            using (var excel = new ExcelPackage())
            {
                action(excel);
                Save(excel,fileName);
            }
            return file;
        }
        /// <summary>
        /// 添加表头
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="headerTexts"></param>
        protected void AddHeader(ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.Length==0)
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="items"></param>
        /// <param name="propSelectors"></param>
        protected void AddDatas<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propSelectors)
        {
            if (propSelectors==null)
            {
                throw new ArgumentNullException(nameof(propSelectors));
            }
            if (items==null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if (items.Count==0)
            {
                return;
            }
            if (propSelectors.Count() == 0)
            {
                //
                return;
            }
            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propSelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propSelectors[j](items[i]);
                }
            }
        }
        
       
        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }
        protected void Save(ExcelPackage excel,string fileName)
        {
            var filePath = Path.Combine(_config.TempFoldPath,fileName);
            //如果文件夹不存在
            //创建
            if (!Directory.Exists(_config.TempFoldPath))
            {
                Directory.CreateDirectory(_config.TempFoldPath);
            }
            excel.SaveAs(new FileInfo(filePath));
        }
    }
}
