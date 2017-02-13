// --------------------------------------------------------------------------
// <copyright file="ExcelStyleTemplateByNPOI.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.11.19">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using System.CodeDom.Compiler;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExcelStyleTemplateByNPOI
    {
        public ExcelStyleTemplateByNPOI(HSSFWorkbook excelFile)
        {
            ExcelFile = excelFile;
        }

        public short BodyColorIndex
        {
            get
            {
                return HSSFColor.White.Index;
            }
        }

        public HSSFWorkbook ExcelFile { get; set; }

        public ICellStyle HeaderTextStyle { get; set; }

        public ICellStyle TextStyle { get; set; }

        public ICellStyle IntStyle { get; set; }

        public ICellStyle DecimalStyle { get; set; }

        public ICellStyle DecimalColorStyle { get; set; }

        public void InitHeaderStyle()
        {
            HeaderTextStyle = NpoiExcelExtensions.HeaderStyle(ExcelFile);
        }

        public void InitBodyStyle()
        {
            TextStyle = NpoiExcelExtensions.TextStyle(ExcelFile, BodyColorIndex, false, Alignment.Left);
            IntStyle = NpoiExcelExtensions.IntStyle(ExcelFile, BodyColorIndex, false);
            DecimalStyle = NpoiExcelExtensions.DecimalStyle(ExcelFile, BodyColorIndex, false);
            DecimalColorStyle = NpoiExcelExtensions.DecimalColorStyle(ExcelFile, BodyColorIndex, false);
        }

        public void InitAllGeneralStyles()
        {
            InitHeaderStyle();
            InitBodyStyle();
        }
    }
}
