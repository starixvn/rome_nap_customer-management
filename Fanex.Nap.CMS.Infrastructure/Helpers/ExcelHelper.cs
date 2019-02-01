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
    using System.IO;
    using System.Web;
    using NPOI.SS.UserModel;
    using OfficeOpenXmlCrypto;

    public static class ExcelHelper
    {
        public static void ResponseStream(IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", fileName));
            workbook.Write(response.OutputStream);
        }

        public static void ResponseStreamWithPassword(IWorkbook workbook, string fileName)
        {
            var response = HttpContext.Current.Response;

            var msBuffer = new MemoryStream();
            workbook.Write(msBuffer);

            var ms = new MemoryStream();
            ms.Write(msBuffer.ToArray(), 0, msBuffer.ToArray().Length);
            byte[] byteReturn = SetPassword(ms);

            GenerateByteToExcel(
                response,
                byteReturn,
                string.Format("{0}.xlsx", fileName),
                "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public static string FormatFileName(string fileName)
        {
            return string.IsNullOrEmpty(fileName) ?
                string.Empty :
                fileName.Replace("/", "_").Replace(" ", "_");
        }

        public static byte[] SetPassword(Stream str)
        {
            var ms = new MemoryStream();
            var buffer = new byte[16 * 1024];
            int iRead;
            str.Position = 0;
            var ocs = new OfficeCryptoStream(str, string.Empty);
            //ocs.Password = "1234aa";
            ocs.Save();
            ocs.WriteTo(str);
            str.Position = 0;

            while ((iRead = str.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, iRead);
            }

            return ms.ToArray();
        }

        public static void GenerateByteToExcel(
            HttpResponse response,
            byte[] fileContent,
            string sFileName,
            string sContentType)
        {
            response.Clear();
            response.ClearHeaders();
            response.ClearContent();
            response.AddHeader("content-disposition", string.Format("attachment; filename={0}", sFileName));
            response.ContentType = sContentType;
            response.BinaryWrite(fileContent);
            //response.Flush();
            response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
