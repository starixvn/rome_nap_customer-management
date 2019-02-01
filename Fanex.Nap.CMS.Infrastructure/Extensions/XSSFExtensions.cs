namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
    using Models;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;
    using NPOI.XSSF.UserModel.Extensions;

    public static class XSSFExtensions
    {
        public const int FontSize = 8;
        private const int VerticalAlignCenter = 2;
        private const string FontName = "Tahoma";
        private const string IntNumberFormat = "#,##0";
        private const string TwoDecimalDigitNumber = "##,##0.00";
        private const string NegativePositiveTwoDecimalDigitNumber = "[>=0]##,##0.00;[RED][<0]-##,##0.00";

        private static XSSFColor HeaderColor = new XSSFColor(new byte[] { 60, 94, 165 });
        private static XSSFColor HeaderFontColor = new XSSFColor(new byte[] { 0, 0, 0 });
        private static XSSFColor BorderColor = new XSSFColor(new byte[] { 0, 0, 0 });
        private static XSSFColor BodyFontColor = new XSSFColor(new byte[] { 255, 255, 255 });

        private static XSSFCellStyle SetBorderToCell(XSSFCellStyle cellStyle)
        {
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
            cellStyle.BorderTop = BorderStyle.Thin;

            cellStyle.SetBorderColor(BorderSide.BOTTOM, BorderColor);
            cellStyle.SetBorderColor(BorderSide.LEFT, BorderColor);
            cellStyle.SetBorderColor(BorderSide.RIGHT, BorderColor);
            cellStyle.SetBorderColor(BorderSide.TOP, BorderColor);

            return cellStyle;
        }

        public static ICellStyle HeaderStyle(XSSFWorkbook excelFile)
        {
            var cellStyle = (XSSFCellStyle)excelFile.CreateCellStyle();
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;

            var font = (XSSFFont)excelFile.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.SetColor(HeaderFontColor);
            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;

            cellStyle.SetFillForegroundColor(HeaderColor);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        public static ICellStyle TextStyle(
            XSSFWorkbook excelFile,
            XSSFColor cellColor,
            bool bold,
            Alignment align,
            XSSFColor fontColor = null)
        {
            fontColor = fontColor ?? BodyFontColor;

            var cellStyle = (XSSFCellStyle)excelFile.CreateCellStyle();
            var font = (XSSFFont)excelFile.CreateFont();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.SetColor(fontColor);

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.SetFillForegroundColor(cellColor);
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

        public static ICellStyle IntStyle(
            XSSFWorkbook excelFile,
            XSSFColor cellColor,
            bool bold,
            XSSFColor fontColor = null)
        {
            return StyleWithFormula(excelFile, cellColor, bold, IntNumberFormat, fontColor);
        }

        public static ICellStyle StyleWithFormula(
            XSSFWorkbook excelFile,
            XSSFColor cellColor,
            bool bold,
            string dataFormula,
            XSSFColor fontColor = null)
        {
            fontColor = fontColor ?? BodyFontColor;

            var cellStyle = (XSSFCellStyle)excelFile.CreateCellStyle();
            var font = (XSSFFont)excelFile.CreateFont();
            IDataFormat dataFormat = excelFile.CreateDataFormat();

            font.FontHeightInPoints = FontSize;
            font.FontName = FontName;
            font.SetColor(fontColor);

            if (bold)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.SetFillForegroundColor(cellColor);
            cellStyle.FillPattern = FillPattern.SolidForeground;
            cellStyle.Alignment = HorizontalAlignment.Right;
            cellStyle.VerticalAlignment = (VerticalAlignment)VerticalAlignCenter;
            cellStyle.DataFormat = dataFormat.GetFormat(dataFormula);
            cellStyle.SetFont(font);
            cellStyle = SetBorderToCell(cellStyle);
            return cellStyle;
        }

        public static ICellStyle DecimalStyle(
            XSSFWorkbook excelFile,
            XSSFColor cellColor,
            bool bold,
            XSSFColor fontColor = null)
        {
            return StyleWithFormula(excelFile, cellColor, bold, TwoDecimalDigitNumber, fontColor);
        }

        public static ICellStyle DecimalColorStyle(
            XSSFWorkbook excelFile,
            XSSFColor cellColor,
            bool bold,
            XSSFColor fontColor = null)
        {
            return StyleWithFormula(excelFile, cellColor, bold, NegativePositiveTwoDecimalDigitNumber, fontColor);
        }
    }
}
