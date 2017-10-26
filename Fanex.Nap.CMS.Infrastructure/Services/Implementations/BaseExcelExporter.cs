namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System.Linq;
    using Helpers;
    using Interfaces;
    using Models;
    using NPOI.HSSF.UserModel;
    using Web;

    public abstract class BaseExcelExporter : IExcelExporter
    {
        private const int TitleHeight = 400;

        private readonly ExportExcelInfoByNPOI _exportExcel;
        private readonly ExcelStyleTemplateByNPOI _excelStyleTemplate;

        protected BaseExcelExporter()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            _exportExcel = new ExportExcelInfoByNPOI
            {
                ExcelFile = workbook,
            };

            _excelStyleTemplate = new ExcelStyleTemplateByNPOI(workbook);
            _excelStyleTemplate.InitAllGeneralStyles();
        }

        protected ExportExcelInfoByNPOI ExportExcel
        {
            get
            {
                return _exportExcel;
            }
        }

        protected ExcelStyleTemplateByNPOI ExcelStyleTemplate
        {
            get
            {
                return _excelStyleTemplate;
            }
        }

        public abstract string FileNameWithoutExtension { get; }

        public virtual string SheetName
        {
            get
            {
                return "Sheet1";
            }
        }

        public virtual string Title
        {
            get
            {
                return string.Empty;
            }
        }

        public abstract ExcelColumn[] ExcelColumns { get; }

        public void ResponseStream()
        {
            Export();

            ExcelHelper.ResponseStream(_exportExcel.ExcelFile, ExcelHelper.FormatFileName(FileNameWithoutExtension));
        }

        protected virtual void Export()
        {
            _exportExcel.Worksheet = _exportExcel.ExcelFile.CreateSheet(SheetName);

            ExportHeader();
            ExportBody();
            ExportFooter();
        }

        protected virtual void ExportHeader()
        {
            ExcelColumn[] excelColumns = ExcelColumns;

            _exportExcel.MaxColumnIndex = excelColumns.Length - 1;
            _exportExcel.SetColumnWidthByFontWidth(excelColumns.Select(c => c.Width).ToArray());

            if (!string.IsNullOrEmpty(Title))
            {
                _exportExcel.CreateCurrentNPOIRow();
                _exportExcel.SetCurrentRowHeight(TitleHeight);

                _exportExcel.RenderRow(Title, _excelStyleTemplate.HeaderTextStyle);

                _exportExcel.RenderEmptyRow(_excelStyleTemplate.TextStyle);
            }

            _exportExcel.CreateCurrentNPOIRow();
            _exportExcel.ResetCurrentColumn();

            foreach (ExcelColumn column in excelColumns)
            {
                _exportExcel.RenderCurrentCell(column.Name, _excelStyleTemplate.HeaderTextStyle);
            }

            _exportExcel.CurrentRow++;
        }

        protected abstract void ExportBody();

        protected virtual void ExportFooter()
        {
        }
    }
}
