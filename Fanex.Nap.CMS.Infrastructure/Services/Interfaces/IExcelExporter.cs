namespace Fanex.Nap.CMS.Infrastructure.Services.Interfaces
{
    using Models;

    public interface IExcelExporter
    {
        string FileNameWithoutExtension { get; }

        string SheetName { get; }

        string Title { get; }

        ExcelColumn[] ExcelColumns { get; }

        void ResponseStream();

        void ResponseCsvStream();
    }
}