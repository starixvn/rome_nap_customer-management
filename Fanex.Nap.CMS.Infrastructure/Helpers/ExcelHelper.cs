// --------------------------------------------------------------------------
// <copyright file="ExcelHelper.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Helpers
{
    using NPOI.SS.UserModel;
    using System.Web;

    /// <summary>
    /// Excel Helper.
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Exports Excel file to response stream.
        /// </summary>
        /// <param name="workbook">The workbook contains the data.</param>
        /// <param name="fileName">The file name.</param>
        public static void ResponseStream(IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            workbook.Write(response.OutputStream);
        }

        public static void ResponseCsvStream(byte[] data, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "text/csv";
            response.AddHeader("Content-Disposition", $"attachment;filename={fileName}.csv");
            response.BinaryWrite(data);
            response.End();
        }

        public static string FormatFileName(string fileName)
        {
            return string.IsNullOrEmpty(fileName) ?
                string.Empty :
                fileName.Replace("/", "_").Replace(" ", "_");
        }
    }
}