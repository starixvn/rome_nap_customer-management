namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System.Linq;
    using Helpers;
    using Interfaces;
    using Models;
    using NPOI.XSSF.UserModel;
    using Web;

    public abstract class XSSFExporter : IExcelExporter
    {
        private const int TitleHeight = 400;

        protected bool _passwordProtection;
        private readonly XSSFFileModel _exportExcel;
        private readonly XSSFTemplate _excelStyleTemplate;

        protected XSSFExporter()
        {
            var workbook = new XSSFWorkbook();
            _excelStyleTemplate = new XSSFTemplate(workbook);
            _excelStyleTemplate.InitAllGeneralStyles();
            _exportExcel = new XSSFFileModel
            {
                ExcelFile = workbook
            };
        }

        protected XSSFFileModel ExportExcel => _exportExcel;

        protected XSSFTemplate ExcelStyleTemplate => _excelStyleTemplate;

        public abstract string FileNameWithoutExtension { get; }

        public virtual string SheetName => "Sheet1";

        public virtual string Title => string.Empty;

        public abstract ExcelColumn[] ExcelColumns { get; }

        public void ResponseStream()
        {
            Export();

            if (_passwordProtection)
            {
                ExcelHelper.ResponseStreamWithPassword(_exportExcel.ExcelFile, ExcelHelper.FormatFileName(FileNameWithoutExtension));
            }
            else
            {
                ExcelHelper.ResponseStream(_exportExcel.ExcelFile, ExcelHelper.FormatFileName(FileNameWithoutExtension));
            }
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
                _exportExcel.CreateCurrentRow();
                _exportExcel.SetCurrentRowHeight(TitleHeight);

                _exportExcel.RenderRow(Title, _excelStyleTemplate.HeaderTextStyle);

                _exportExcel.RenderEmptyRow(_excelStyleTemplate.TextStyle);
            }

            _exportExcel.CreateCurrentRow();
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
