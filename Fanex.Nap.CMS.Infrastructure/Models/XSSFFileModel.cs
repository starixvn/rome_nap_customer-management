namespace Fanex.Nap.CMS.Infrastructure.Models
{
    using Extensions;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NPOI.XSSF.UserModel;
    using Resources;

    public class XSSFFileModel
    {
        public XSSFWorkbook ExcelFile { get; set; }

        public ISheet Worksheet { get; set; }

        public int MaxColumnIndex { get; set; }

        public int StartColumn { get; set; }

        public int CurrentRow { get; set; }

        public int CurrentColumn { get; set; }

        public IRow Row { get; set; }

        public void SetColumnWidthByFontWidth(int[] columnWidths)
        {
            var currentColumn = StartColumn;
            for (var i = 0; i < columnWidths.Length; i++, currentColumn++)
            {
                var width = columnWidths[i];
                Worksheet.SetColumnWidth(currentColumn, width * NpoiExcelExtensions.FontWidth);
            }
        }

        public void CreateCurrentRow()
        {
            Row = Worksheet.GetRow(CurrentRow) ?? Worksheet.CreateRow(CurrentRow);
        }

        public void SetCurrentRowHeight(short height)
        {
            Row.Height = height;
        }

        public void ResetCurrentColumn()
        {
            CurrentColumn = StartColumn;
        }

        public void RenderRow(object cellValue, ICellStyle cellStyle)
        {
            CreateCurrentRow();
            ResetCurrentColumn();
            RenderCell(CurrentRow, CurrentRow, MaxColumnIndex, cellValue, cellStyle);
            CurrentRow++;
        }

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

            if (startRow != endRow || CurrentColumn != endColumn)
            {
                Worksheet.AddMergedRegion(new CellRangeAddress(startRow, endRow, CurrentColumn, endColumn));
            }

            CurrentColumn = endColumn + 1;
        }

        public static void SetCellValue(ICell cell, object cellValue)
        {
            NpoiExcelExtensions.SetCellValue(cell, cellValue);
        }

        public void RenderEmptyRow(ICellStyle cellStyle)
        {
            CreateCurrentRow();
            ResetCurrentColumn();
            RenderRow(string.Empty, cellStyle);
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

        public void RenderEmptyReport(ICellStyle bodyStyle)
        {
            CreateCurrentRow();
            ResetCurrentColumn();
            RenderRow(Labels.Message_NoReportData, bodyStyle);
        }
    }
}
