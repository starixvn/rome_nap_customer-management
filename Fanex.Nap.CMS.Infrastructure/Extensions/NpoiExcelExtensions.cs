namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
    using System;
    using System.Drawing;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// NPOI Excel Extensions.
    /// </summary>
    public static class NpoiExcelExtensions
    {
        public const int FontWidth = 30;
        public const int FontHeight = 20;
        public const int FontSize = 8;
        public const int MaxRowPerSheet = 20000;

        public static readonly Color BackGroundColor = Color.White;
        public static readonly Color ReportTitleColor = Color.FromArgb(255, 217, 97);
        public static readonly Color HeaderTitleColor = Color.FromArgb(60, 94, 165);
        public static readonly Color EvenRowColor = Color.FromArgb(228, 242, 253);
        public static readonly Color OddRowColor = Color.White;
        public static readonly Color SubTotalRowColor = Color.FromArgb(244, 244, 190);
        public static readonly Color TotalRowColor = Color.FromArgb(244, 244, 190);
        public static readonly Color GrandTotalRowColor = Color.FromArgb(219, 225, 187);
        public static readonly Color GrandTotalSeparatorColor = Color.FromArgb(255, 102, 0);
        public static readonly Color RevenueLicenseeColor = Color.FromArgb(213, 132, 52);
        public static readonly Color ParentColor = Color.FromArgb(194, 222, 236);
        public static readonly Color ChildColor = Color.FromArgb(194, 226, 236);
        public static readonly Color NoteInformationCellColor = Color.FromArgb(181, 196, 208);
        public static readonly Color BorderColor = Color.Gray;

        private const string FontName = "Tahoma";
        private const string IntNumberFormat = "#,##0";
        private const string TwoDecimalDigitNumber = "##,##0.00";
        private const string NegativePositiveTwoDecimalDigitNumber = "[>=0]##,##0.00;[RED][<0]-##,##0.00";

        private const int VerticalAlignCenter = 1;

        /// <summary>
        /// Style for header.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle HeaderStyle(HSSFWorkbook excelFile)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            HSSFPalette palette = excelFile.GetCustomPalette();
            IFont font = excelFile.CreateFont();
            palette.SetColorAtIndex(HSSFColor.Pink.Index, (byte)60, (byte)94, (byte)165);
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.White.Index;
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;

            cellStyle.FillForegroundColor = HSSFColor.Pink.Index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Style for text.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The color of cell.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="align">Align text(value is right, left or center).</param>
        /// <param name="fontColor">The color of font.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle TextStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            Alignment align,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;

            switch (align)
            {
                case Alignment.Right:
                    cellStyle.Alignment = HorizontalAlignment.Right;
                    break;
                case Alignment.Left:
                    cellStyle.Alignment = HorizontalAlignment.Left;
                    break;
                case Alignment.Center:
                    cellStyle.Alignment = HorizontalAlignment.Center;
                    break;
            }

            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Style for decimal value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle DecimalStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            short fontColor = 8)
        {
            return StyleWithFormula(excelFile, cellColor, bold, TwoDecimalDigitNumber, fontColor);
        }

        public static ICellStyle DecimalColorStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            short fontColor = 8)
        {
            return StyleWithFormula(excelFile, cellColor, bold, NegativePositiveTwoDecimalDigitNumber, fontColor);
        }

        /// <summary>
        /// Style for integer value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle IntStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            short fontColor = 8)
        {
            return StyleWithFormula(excelFile, cellColor, bold, IntNumberFormat, fontColor);
        }

        /// <summary>
        /// Style for integer value.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="align">Align text(value is right, left or center).</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>The cell style.</returns>
        public static ICellStyle IntStyle(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            Alignment align,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            IDataFormat dataFormat = excelFile.CreateDataFormat();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.Alignment = ConvertAlignment(align, cellStyle.Alignment);
            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.DataFormat = dataFormat.GetFormat(IntNumberFormat);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The cell of row.</param>
        /// <param name="cellValue">The cell value.</param>
        public static void SetCellValue(ICell cell, object cellValue)
        {
            if (cellValue is string)
            {
                cell.SetCellValue(Convert.ToString(cellValue));
            }
            else if (cellValue is int)
            {
                cell.SetCellValue(Convert.ToInt32(cellValue));
            }
            else if (cellValue is long)
            {
                cell.SetCellValue(Convert.ToInt64(cellValue));
            }
            else if (cellValue is double || cellValue is decimal)
            {
                cell.SetCellValue(Convert.ToDouble(cellValue));
            }
            else if (cellValue is bool)
            {
                cell.SetCellValue(Convert.ToBoolean(cellValue));
            }
            else if (cellValue is System.DateTime)
            {
                cell.SetCellValue(Convert.ToDateTime(cellValue));
            }
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="row">The row of report.</param>
        /// <param name="currentColumn">The current column.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>Current column index.</returns>
        public static int RenderCell(IRow row, int currentColumn, object cellValue, ICellStyle cellStyle)
        {
            ICell cell = row.CreateCell(currentColumn);
            SetCellValue(cell, cellValue);
            cell.CellStyle = cellStyle;
            currentColumn += 1;
            return currentColumn;
        }

        /// <summary>
        /// Set border to cell.
        /// </summary>
        /// <param name="cellStyle">The cell style.</param>
        /// <returns>The style of cell.</returns>
        private static ICellStyle SetBorderToCell(ICellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.BottomBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.LeftBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.RightBorderColor = HSSFColor.Grey80Percent.Index;
            cellStyle.TopBorderColor = HSSFColor.Grey80Percent.Index;

            return cellStyle;
        }

        /// <summary>
        /// Get style with formula.
        /// </summary>
        /// <param name="excelFile">The excel file.</param>
        /// <param name="cellColor">The cell color.</param>
        /// <param name="bold">Check bold.</param>
        /// <param name="dataFormula">The data formula.</param>
        /// <param name="fontColor">The font color.</param>
        /// <returns>
        /// The cell style.
        /// </returns>
        private static ICellStyle StyleWithFormula(
            HSSFWorkbook excelFile,
            short cellColor,
            bool bold,
            string dataFormula,
            short fontColor = 8)
        {
            ICellStyle cellStyle = excelFile.CreateCellStyle();
            IFont font = excelFile.CreateFont();
            IDataFormat dataFormat = excelFile.CreateDataFormat();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.Color = fontColor;

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.FillForegroundColor = cellColor;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.DataFormat = dataFormat.GetFormat(dataFormula);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        /// <summary>
        /// Converts the alignment.
        /// </summary>
        /// <param name="alignment">The alignment.</param>
        /// <param name="currentAlign">The current align.</param>
        /// <returns>The horizontal alignment.</returns>
        private static HorizontalAlignment ConvertAlignment(
            Alignment alignment,
            HorizontalAlignment currentAlign)
        {
            switch (alignment)
            {
                case Alignment.Right:
                    return HorizontalAlignment.Right;
                case Alignment.Left:
                    return HorizontalAlignment.Left;
                case Alignment.Center:
                    return HorizontalAlignment.Center;
                default:
                    return currentAlign;
            }
        }
    }
}
