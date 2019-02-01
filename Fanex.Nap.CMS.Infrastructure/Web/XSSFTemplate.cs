namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using Extensions;
    using Models;
    using NPOI.SS.UserModel;
    using NPOI.XSSF.UserModel;

    public class XSSFTemplate
    {
        public readonly XSSFColor BodyColor = new XSSFColor(new byte[] { 255, 255, 255 });

        public XSSFTemplate(XSSFWorkbook excelFile)
        {
            ExcelFile = excelFile;
        }

        public XSSFWorkbook ExcelFile { get; set; }

        public ICellStyle HeaderTextStyle { get; set; }

        public ICellStyle TextStyle { get; set; }

        public ICellStyle IntStyle { get; set; }

        public ICellStyle DecimalStyle { get; set; }

        public ICellStyle DecimalColorStyle { get; set; }

        public void InitAllGeneralStyles()
        {
            InitHeaderStyle();
            InitBodyStyle();
        }

        public void InitHeaderStyle()
        {
            HeaderTextStyle = XSSFExtensions.HeaderStyle(ExcelFile);
        }

        public void InitBodyStyle()
        {
            TextStyle = XSSFExtensions.TextStyle(ExcelFile, BodyColor, false, Alignment.Left);
            IntStyle = XSSFExtensions.IntStyle(ExcelFile, BodyColor, false);
            DecimalStyle = XSSFExtensions.DecimalStyle(ExcelFile, BodyColor, false);
            DecimalColorStyle = XSSFExtensions.DecimalColorStyle(ExcelFile, BodyColor, false);
        }
    }
}
