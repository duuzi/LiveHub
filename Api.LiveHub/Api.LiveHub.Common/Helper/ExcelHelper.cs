using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace Api.LiveHub.Common.Helper
{
    public class ExcelHelper
    {
        public static byte[] Output(DataTable dataTable, string[] tableTitle)
        {
            NPOI.SS.UserModel.IWorkbook workbook = new NPOI.XSSF.UserModel.XSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = workbook.CreateSheet("sheet");
            IRow Title = null;
            IRow rows = null;
            for (int i = 1; i <= dataTable.Rows.Count; i++)
            {
                //创建表头
                if (i - 1 == 0)
                {
                    Title = sheet.CreateRow(0);
                    for (int k = 1; k < tableTitle.Length + 1; k++)
                    {
                        Title.CreateCell(0).SetCellValue("序号");
                        Title.CreateCell(k).SetCellValue(tableTitle[k - 1]);
                    }
                    continue;
                }
                else
                {
                    rows = sheet.CreateRow(i - 1);
                    for (int j = 1; j <= dataTable.Columns.Count; j++)
                    {
                        rows.CreateCell(0).SetCellValue(i - 1);
                        rows.CreateCell(j).SetCellValue(dataTable.Rows[i - 1][j - 1].ToString());
                    }
                }
            }

            byte[] buffer = new byte[1024 * 5];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }
            return buffer;
        }

        public static byte[] OutputExcel<T>(List<T> entitys, string[] title)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            IRow Title = null;
            IRow rows = null;
            Type entityType = entitys.Count>0?entitys[0].GetType():typeof(string);
            PropertyInfo[] entityProperties = entityType.GetProperties();

            for (int i = 0; i <= entitys.Count; i++)
            {
                if (i == 0)
                {
                    Title = sheet.CreateRow(0);
                    for (int k = 1; k < title.Length + 1; k++)
                    {
                        Title.CreateCell(0).SetCellValue("序号");
                        Title.CreateCell(k).SetCellValue(title[k - 1]);
                    }

                    continue;
                }
                else
                {
                    rows = sheet.CreateRow(i);
                    object entity = entitys[i - 1];
                    for (int j = 1; j <= entityProperties.Length; j++)
                    {
                        object[] entityValues = new object[entityProperties.Length];
                        entityValues[j - 1] = entityProperties[j - 1].GetValue(entity);
                        rows.CreateCell(0).SetCellValue(i);
                        rows.CreateCell(j).SetCellValue(entityValues[j - 1]==null? "":entityValues[j - 1].ToString());
                    }
                }
            }

            byte[] buffer = new byte[1024 * 2];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.GetBuffer();
                ms.Close();
            }

            return buffer;
        }
    }
}
