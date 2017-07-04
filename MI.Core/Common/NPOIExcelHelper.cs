using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Common
{
    /// <summary>
    /// 导出EXCEL辅助类
    /// </summary>
    public class NPOIExcelHelper
    {
        /// <summary>
        /// 要求输入的中文说明来源，Inner来自于数据表的中文说明自动读取，None表示只读英文
        /// </summary>
        public enum ExtendPropertiesType
        {
            /// <summary>
            /// 中文说明
            /// </summary>
            Inner,
            /// <summary>
            /// None表示只读英文
            /// </summary>
            None
        };

        //导出一个xls文件，若干个sheet
        private static void CreateExcel(out HSSFWorkbook workbook, DataSet ds, List<string[]> LheaderList)
        {
            workbook = new HSSFWorkbook();
            for (int t = 0; t < ds.Tables.Count; t++)
            {
                //创建SHEET对象
                DataTable dt = ds.Tables[t];
                string[] headerList = LheaderList == null ? new string[] { } : LheaderList[t];
                ISheet sheet = workbook.CreateSheet(dt.TableName);

                //设置单元格样式
                ICellStyle style = workbook.CreateCellStyle();
                style.BorderBottom = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderTop = BorderStyle.Thin;
                style.WrapText = true;//换行
                //设置标题行字体
                IFont font = workbook.CreateFont();
                font.FontName = "宋体";
                font.FontHeightInPoints = 12;
                font.Color = HSSFColor.Red.Index;
                font.Boldweight = 700;

                //设置标题行单元格样式
                ICellStyle hstyle = workbook.CreateCellStyle();
                hstyle.BorderBottom = BorderStyle.Thin;
                hstyle.BorderLeft = BorderStyle.Thin;
                hstyle.BorderRight = BorderStyle.Thin;
                hstyle.BorderTop = BorderStyle.Thin;
                hstyle.WrapText = true;//换行
                hstyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                hstyle.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
                hstyle.SetFont(font);

                //生成标题行
                int colnum = 0;
                IRow rowHeader = sheet.CreateRow(0);

                if (headerList != null && headerList.Length == dt.Columns.Count)
                {
                    foreach (string str in headerList)
                    {
                        //设置宽度
                        sheet.SetColumnWidth(colnum, 4000);
                        //设置标题
                        ICell cell = rowHeader.CreateCell(colnum, CellType.String);
                        cell.SetCellValue(str);
                        cell.CellStyle = hstyle;
                        colnum++;
                    }
                }
                else
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //设置宽度
                        sheet.SetColumnWidth(colnum, 4000);
                        //设置标题
                        ICell cell = rowHeader.CreateCell(colnum, CellType.String);
                        cell.SetCellValue(dc.ColumnName);
                        cell.CellStyle = hstyle;
                        colnum++;
                    }
                }
                //生成数据行
                int rownum = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    IRow row = sheet.CreateRow(rownum);
                    row.Height = 600;
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string drValue = dr[i].ToString();
                        CellType ct = TranDataType(dt.Columns[i].DataType.ToString());
                        ICell cell = row.CreateCell(i, ct);
                        cell.CellStyle = style;
                        if (ct == CellType.Numeric)
                        {
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            cell.SetCellValue(doubV);
                        }
                        else if (ct == CellType.Boolean)
                        {
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            cell.SetCellValue(boolV);
                        }
                        else
                        {
                            cell.SetCellValue(drValue);
                        }
                    }
                    rownum++;
                }
            }
        }

        /// <summary>
        /// 导出excel文件，配置输入字段标题列表
        /// </summary>
        /// <param name="Response">响应流</param>
        /// <param name="fileName">想要保存的文件名</param>
        /// <param name="ds">要导出的数据集</param>
        /// <param name="LheaderList">字段标题列表的集</param>
        public static MemoryStream ExportExcel(DataSet ds, List<string[]> LheaderList)
        {

            //创建EXCEL表格对象
            HSSFWorkbook workbook = new HSSFWorkbook();
            CreateExcel(out workbook, ds, LheaderList);

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;

            #region 使用using会抛异常：System.ObjectDisposedException: 无法访问已关闭的流
            /*
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //sheet.Dispose();
                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
           */
            #endregion
        }

        /// <summary>
        /// 导出excel文件，可选导出英文字段名称或者中文说明，其中中文说明可选数据表内置中文说明或者FieldInfo的配置表，可选数据库连接
        /// </summary>
        /// <param name="Response">响应流</param>
        /// <param name="fileName">想要保存的文件名</param>
        /// <param name="ds">要导出的数据集</param>
        /// <param name="type">设置中文字段类型</param>
        /// <param name="TableNameList">数据库中的数据表名称集</param>
        /// <param name="ConnStrList">TableNameList对应的数据库连接</param>
        public static MemoryStream ExportExcel(DataSet ds, ExtendPropertiesType type, string[] TableNameList, string[] ConnStrList)
        {
            switch (type)
            {
                case ExtendPropertiesType.Inner:
                    break;
                case ExtendPropertiesType.None:
                default:
                    return ExportExcel(ds, null);
            }
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                string TableName = TableNameList[i];
                string ConnStr = "";
                DataTable dtEP = ReadEPorFI(type, ConnStr, TableName);
                foreach (DataRow dr in dtEP.Rows)
                {
                    string ColName = dr["cloname"].ToString();
                    string EP = dr["colDecr"].ToString();
                    if (EP != null && EP != "")
                    {
                        dt.Columns[ColName].ColumnName = EP;
                    }
                }
            }
            return ExportExcel(ds, null);
        }

        /// <summary>
        /// 导出excel文件，可选导出英文字段名称或者中文说明，其中中文说明可选数据表内置中文说明或者FieldInfo的配置表，可选数据库连接
        /// </summary>
        /// <param name="Response">响应流</param>
        /// <param name="fileName">想要保存的文件名</param>
        /// <param name="ds">要导出的数据集</param>
        /// <param name="type">设置中文字段类型</param>
        /// <param name="TableNameList">数据库中的数据表名称集</param>
        /// <param name="dtColumns">表的列中文描述</param>
        public static MemoryStream ExportExcel(DataSet ds, ExtendPropertiesType type, string[] TableNameList, DataTable dtColumns)
        {
            switch (type)
            {
                case ExtendPropertiesType.Inner:
                    break;
                case ExtendPropertiesType.None:
                default:
                    return ExportExcel(ds, null);
            }
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                string TableName = TableNameList[i];
                DataTable dtEP = dtColumns;
                foreach (DataRow dr in dtEP.Rows)
                {
                    string ColName = dr["cloname"].ToString();
                    string EP = dr["colDecr"].ToString();
                    if (EP != null && EP != "")
                    {
                        dt.Columns[ColName].ColumnName = EP;
                    }
                }
            }
            return ExportExcel(ds, null);
        }

        /// <summary>
        /// 导出excel文件，可选导出英文字段名称或者中文说明，其中中文说明可选数据表内置中文说明或者FieldInfo的配置表
        /// </summary>
        /// <param name="Response">响应流</param>
        /// <param name="dt">要导出的数据表</param>
        /// <param name="type">设置中文字段类型</param>
        /// <param name="TableName">数据库中的数据表名称</param>
        public static MemoryStream ExportExcel(DataTable dt, ExtendPropertiesType type, string TableName)
        {
            return ExportExcel(dt.DataSet, type, new string[] { TableName }, new string[] { });
        }

        /// <summary>
        /// 导出excel文件，可选导出英文字段名称或者中文说明，其中中文说明可选数据表内置中文说明或者FieldInfo的配置表
        /// </summary>
        /// <param name="Response">响应流</param>
        /// <param name="dt">要导出的数据表</param>
        /// <param name="type">设置中文字段类型</param>
        /// <param name="TableName">数据库中的数据表名称</param>
        public static MemoryStream ExportExcel(DataTable dt, ExtendPropertiesType type, string TableName, DataTable dtColumns)
        {
            return ExportExcel(dt.DataSet, type, new string[] { TableName }, dtColumns);
        }

        //用不同方法读取中文说明
        private static DataTable ReadEPorFI(ExtendPropertiesType type, string ConnStr, string TableName)
        {
            string spname = "";
            DataTable dt = null;
            switch (type)
            {
                case ExtendPropertiesType.Inner:

                    spname = "SP_GetExtendPropertiesByTableName";
                    break;
            }
            return dt;
        }


        private static CellType TranDataType(string DataType)
        {
            CellType ct = CellType.String;
            switch (DataType.ToLower())
            {
                case "system.uint64":
                case "system.uint32":
                case "system.uint16":
                case "system.byte":
                case "system.int64":
                case "system.int32":
                case "system.int16":
                case "system.double":
                case "system.single":
                case "system.decimal":
                    ct = CellType.Numeric;
                    break;
                case "system.boolean":
                    ct = CellType.Boolean;
                    break;
                case "system.string":
                case "system.byte[]":
                case "system.datetime":
                default:
                    ct = CellType.String;
                    break;
            }
            return ct;
        }

    }
}
