using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HRL.BLL
{
    public class ExcelHelperEF<T>
    {

        /// <summary>
        /// 泛型集合类导出成excel
        /// </summary>
        /// <param name="list">泛型集合类</param>
        /// <param name="fileName">生成的excel文件名</param>
        /// <param name="modelName">表名</param>
        public static void ListToExcel(List<T> list, string fileName, string tableName, string[] notfield)
        {
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
            HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", fileName));
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.BinaryWrite(ListToExcel<T>(list, tableName, notfield).GetBuffer());
            HttpContext.Current.Response.End();
        }

        public static MemoryStream ListToExcel<T1>(IList<T> list, string tableName, string[] notfield)
        {
            //获取指定表的所有字段
            string[] propertyName = GlobalLngResource.GetColumnsFrmRes(notfield, tableName);

            //创建流对象
            using (MemoryStream ms = new MemoryStream())
            {
                //将参数写入到一个临时集合中
                List<string> propertyNameList = new List<string>();
                List<string> propertyValueList = new List<string>();
                if (propertyName != null)
                    propertyNameList.AddRange(propertyName);
                //根据参数遍历资源文件值
                foreach (string item in propertyName)
                {
                    if (!string.IsNullOrEmpty(GlobalLngResource.GetRes(item, tableName)))
                    {
                        propertyValueList.Add(GlobalLngResource.GetRes(item, tableName));
                    }
                    //if (rm.GetString(item) != null)
                    //    propertyValueList.Add(rm.GetString(item));
                }
                //创NOPI的相关对象
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);
                if (list.Count > 0)
                {
                    //通过反射得到对象的属性集合
                    //  PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    //遍历资源文件值集合生成excel的表头标题
                    for (int i = 0; i < propertyValueList.Count; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(propertyValueList[i].ToString());
                    }
                    int rowIndex = 1;
                    //遍历集合生成excel的行集数据
                    for (int i = 0; i < list.Count; i++)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);
                        for (int j = 0; j < propertyNameList.Count; j++)
                        {
                            //通过反射得到参数对象的属性
                            PropertyInfo property = list[i].GetType().GetProperty(propertyNameList[j].ToString());
                            if (propertyNameList.Contains(property.Name))
                            {
                                object obj = property.GetValue(list[i], null);
                                if (obj != null)
                                    dataRow.CreateCell(j).SetCellValue(obj.ToString());
                            }
                        }
                        rowIndex++;
                    }
                }
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }




        ///// <summary>
        ///// 泛型集合类导出成excel
        ///// </summary>
        ///// <param name="list">泛型集合类</param>
        ///// <param name="fileName">生成的excel文件名</param>
        ///// <param name="propertyName">数据库的字段列表</param>
        ///// <param name="rm">ResourceManager资源管理器对象</param>
        //public static void ListToExcel(List<T> list, string fileName, string[] propertyName, ResourceManager rm)
        //{
        //    HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
        //    HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}.xls", fileName));
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.BinaryWrite(ListToExcel<T>(list, propertyName, rm).GetBuffer());
        //    HttpContext.Current.Response.End();
        //}

        //public static MemoryStream ListToExcel<T1>(IList<T> list, string[] propertyName, ResourceManager rm)
        //{
        //    //创建流对象
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        //将参数写入到一个临时集合中
        //        List<string> propertyNameList = new List<string>();
        //        List<string> propertyValueList = new List<string>();
        //        if (propertyName != null)
        //            propertyNameList.AddRange(propertyName);
        //        //根据参数遍历资源文件值
        //        foreach (string item in propertyName)
        //        {
        //            if (rm.GetString(item) != null)
        //                propertyValueList.Add(rm.GetString(item));
        //        }
        //        //创NOPI的相关对象
        //        IWorkbook workbook = new HSSFWorkbook();
        //        ISheet sheet = workbook.CreateSheet();
        //        IRow headerRow = sheet.CreateRow(0);
        //        if (list.Count > 0)
        //        {
        //            //通过反射得到对象的属性集合
        //            //  PropertyInfo[] propertys = list[0].GetType().GetProperties();
        //            //遍历资源文件值集合生成excel的表头标题
        //            for (int i = 0; i < propertyValueList.Count; i++)
        //            {
        //                headerRow.CreateCell(i).SetCellValue(propertyValueList[i].ToString());
        //            }
        //            int rowIndex = 1;
        //            //遍历集合生成excel的行集数据
        //            for (int i = 0; i < list.Count; i++)
        //            {
        //                IRow dataRow = sheet.CreateRow(rowIndex);
        //                for (int j = 0; j < propertyNameList.Count; j++)
        //                {
        //                    //通过反射得到参数对象的属性
        //                    PropertyInfo property = list[i].GetType().GetProperty(propertyNameList[j].ToString());
        //                    if (propertyNameList.Contains(property.Name))
        //                    {
        //                        object obj = property.GetValue(list[i], null);
        //                        if (obj != null)
        //                            dataRow.CreateCell(j).SetCellValue(obj.ToString());
        //                    }
        //                }
        //                rowIndex++;
        //            }
        //        }
        //        workbook.Write(ms);
        //        ms.Flush();
        //        ms.Position = 0;
        //        return ms;
        //    }
        //}




        //public static System.Data.DataTable ImportExcelToDataTable(int fileID, string httpPath, HttpPostedFileBase file)
        //{
        //    // int fileID = fileUploadBLL.GetMaxID();
        //    //  HttpPostedFileBase file = HttpQuestBase Request.Files[0]; //上传文件
        //    string realFileName = Path.GetFileName(file.FileName);//上传文件名
        //    string extFileName = Path.GetExtension(file.FileName);//文件扩展名
        //    string fileName = fileID.ToString() + extFileName;//转换后保存服务器的文件名
        //    string filePath = Path.Combine(httpPath, fileName);
        //    file.SaveAs(filePath);
        //    System.Data.DataTable dt = UnipecERPBridge.Common.ExcelHelper<System.Data.DataTable>.ImportExcelFile(filePath);
        //    return dt;
        //}


        //private static DataTable ImportExcelFile(string filePath)
        //{
        //    //HSSFWorkbook hssfworkbook;
        //    XSSFWorkbook hssfworkbook;
        //    #region//初始化信息
        //    try
        //    {
        //        using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //        {
        //            hssfworkbook = new XSSFWorkbook(file);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    #endregion

        //    //using (NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0))
        //    //{
        //    ISheet sheet = hssfworkbook.GetSheetAt(0);
        //    DataTable table = new DataTable();
        //    IRow headerRow = sheet.GetRow(0);//第一行为标题行
        //    int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
        //    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1
        //    //handling header.
        //    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
        //    {
        //        DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
        //        table.Columns.Add(column);
        //    }
        //    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
        //    {
        //        IRow row = sheet.GetRow(i);
        //        DataRow dataRow = table.NewRow();
        //        if (row != null)
        //        {
        //            for (int j = row.FirstCellNum; j < cellCount; j++)
        //            {
        //                if (row.GetCell(j) != null)
        //                    dataRow[j] = GetCellValue(row.GetCell(j));
        //            }
        //        }
        //        table.Rows.Add(dataRow);
        //    }
        //    return table;
        //    //}
        //}



        ///// <summary>
        ///// 根据Excel列类型获取列的值
        ///// </summary>
        ///// <param name="cell">Excel列</param>
        ///// <returns></returns>
        //private static string GetCellValue(NPOI.SS.UserModel.ICell cell)
        //{
        //    if (cell == null)
        //        return string.Empty;
        //    switch (cell.CellType)
        //    {
        //        case CellType.Blank:
        //            return string.Empty;
        //        case CellType.Boolean:
        //            return cell.BooleanCellValue.ToString();
        //        case CellType.Error:
        //            return cell.ErrorCellValue.ToString();
        //        case CellType.Numeric:
        //        case CellType.Unknown:
        //        default:
        //            return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
        //        case CellType.String:
        //            return cell.StringCellValue;
        //        case CellType.Formula:
        //            try
        //            {
        //                HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
        //                e.EvaluateInCell(cell);
        //                return cell.ToString();
        //            }
        //            catch
        //            {
        //                return cell.NumericCellValue.ToString();
        //            }
        //    }
        //}

    }
}
