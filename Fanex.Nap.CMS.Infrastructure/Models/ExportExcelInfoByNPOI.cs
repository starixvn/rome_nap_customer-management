// --------------------------------------------------------------------------
// <copyright file="ExportExcelInfoByNPOI.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Models
{
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Export Excel Information.
    /// </summary>
    public class ExportExcelInfoByNPOI
    {
        public int StartRow { get; set; }

        public int StartColumn { get; set; }

        public int CurrentRow { get; set; }

        public int CurrentColumn { get; set; }

        public int MaxColumnIndex { get; set; }

        public HSSFWorkbook ExcelFile { get; set; }

        public ISheet Worksheet { get; set; }

        public IRow Row { get; set; }

        public ICell PreviousCollumnCell
        {
            get
            {
                return Row.GetCell(CurrentColumn - 1);
            }
        }

        /// <summary>
        /// Sets the cell value.
        /// </summary>
        /// <param name="cell">The cell of row.</param>
        /// <param name="cellValue">The cell value.</param>
        public static void SetCellValue(ICell cell, object cellValue)
        {
            NpoiExcelExtensions.SetCellValue(cell, cellValue);
        }

        public void ResetCurrentColumn()
        {
            CurrentColumn = StartColumn;
        }

        public void CreateCurrentNPOIRow()
        {
            Row = Worksheet.GetRow(CurrentRow) ?? Worksheet.CreateRow(CurrentRow);
        }

        public void RenderCurrentCell(object cellValue, ICellStyle cellStyle)
        {
            RenderCell(CurrentRow, CurrentRow, CurrentColumn, cellValue, cellStyle);
        }

        public void RenderCellBySpanning(
            int rowSpan,
            int columnSpan,
            object cellValue,
            ICellStyle cellStyle)
        {
            RenderCell(CurrentRow, rowSpan, columnSpan, cellValue, cellStyle);
        }

        /// <summary>
        /// Renders the cell.
        /// </summary>
        /// <param name="startRow">The start row.</param>
        /// <param name="endRow">The end row.</param>
        /// <param name="endColumn">The end column.</param>
        /// <param name="cellValue">The cell value.</param>
        /// <param name="cellStyle">The cell style.</param>
        public void RenderCell(int startRow, int endRow, int endColumn, object cellValue, ICellStyle cellStyle)
        {
            for (var i = startRow; i <= endRow; i++)
            {
                Row = Worksheet.GetRow(i) ?? Worksheet.CreateRow(i);
                for (var j = CurrentColumn; j <= endColumn; j++)
                {
                    ICell cell = Row.GetCell(j) ?? Row.CreateCell(j);

                    if (i == startRow && j == CurrentColumn)
                    {
                        SetCellValue(cell, cellValue);
                    }

                    if (i == startRow ||
                        i == endRow ||
                        j == CurrentColumn ||
                        j == endColumn)
                    {
                        cell.CellStyle = cellStyle;
                    }
                }
            }

            Worksheet.AddMergedRegion(new CellRangeAddress(startRow, endRow, CurrentColumn, endColumn));
            CurrentColumn = endColumn + 1;
        }

        public void RenderRow(object cellValue, ICellStyle cellStyle)
        {
            CreateCurrentNPOIRow();
            ResetCurrentColumn();
            RenderCell(CurrentRow, CurrentRow, MaxColumnIndex, cellValue, cellStyle);
            CurrentRow++;
        }

        public void RenderEmptyRow(ICellStyle cellStyle)
        {
            CreateCurrentNPOIRow();
            ResetCurrentColumn();
            RenderRow(string.Empty, cellStyle);
        }

        public void RenderEmptyReport(ICellStyle bodyStyle)
        {
            CreateCurrentNPOIRow();
            ResetCurrentColumn();
            RenderRow(Labels.Message_NoReportData, bodyStyle);
        }

        public void SetColumnWidth(int columnIndex, int width)
        {
            Worksheet.SetColumnWidth(columnIndex, width);
        }

        public void SetColumnWidthByFontWidth(int columnIndex, int width)
        {
            Worksheet.SetColumnWidth(columnIndex, width * NpoiExcelExtensions.FontWidth);
        }

        public void SetColumnWidthByFontWidth(int[] columnWidths)
        {
            var currentColumn = StartColumn;
            for (var i = 0; i < columnWidths.Length; i++, currentColumn++)
            {
                var width = columnWidths[i];
                Worksheet.SetColumnWidth(currentColumn, width * NpoiExcelExtensions.FontWidth);
            }
        }

        public void SetCurrentRowHeight(short height)
        {
            Row.Height = height;
        }

        public void RenderCurrentCellText(ICellStyle cellStyle, IRichTextString richText)
        {
            Row = Worksheet.GetRow(CurrentRow) ?? Worksheet.CreateRow(CurrentRow);
            ICell cell = Row.GetCell(CurrentColumn) ?? Row.CreateCell(CurrentColumn);
            cell.CellStyle = cellStyle;
            cell.SetCellValue(richText);

            CurrentColumn++;
        }
    }
}
