namespace Fanex.NAP.CMS.Controllers.Exporter
{
    using System.Linq;
    using Core.Resources;
    using Models.StatusLog;
    using Nap.CMS.Infrastructure.Extensions;
    using Nap.CMS.Infrastructure.Models;
    using Nap.CMS.Infrastructure.Services.Implementations;
    using Nap.CMS.Infrastructure.Services.Interfaces;

    public class StatusLogExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly StatusLogReport report;

        public StatusLogExporter(StatusLogReport report)
        {
            this.report = report;
        }

        public override string FileNameWithoutExtension
        {
            get
            {
                return Core.Resources.ViewLang.StatusLog;
            }
        }

        public override string SheetName
        {
            get
            {
                return Core.Resources.ViewLang.StatusLog;
            }
        }

        public override string Title
        {
            get
            {
                return Core.Resources.ViewLang.StatusLog;
            }
        }

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                var result = new ExcelColumn[5];
                result[0] = new ExcelColumn(ViewLang.LogID, 100);
                result[1] = new ExcelColumn(ViewLang.LogDate, 100);
                result[2] = new ExcelColumn(ViewLang.EditedBy, 200);
                result[3] = new ExcelColumn(ViewLang.Action, 300);
                result[4] = new ExcelColumn(ViewLang.Detail, 200);

                return result;
            }
        }

        protected override void ExportBody()
        {
            if (report.Logs.Any())
            {
                var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

                foreach (var log in report.Logs)
                {
                    ExportExcel.ResetCurrentColumn();
                    ExportExcel.RenderCurrentCell(log.LogID, centerTextStyle);
                    ExportExcel.RenderCurrentCell(log.LogDateShortDate + "\n\r" + log.LogDateLongTime, centerTextStyle);
                    ExportExcel.RenderCurrentCell(log.EditorName + "\n\r(" + log.EditorId + ")", centerTextStyle);
                    ExportExcel.RenderCurrentCell(log.Action + "\n\r(" + log.SPName + ")", ExcelStyleTemplate.TextStyle);

                    var detail = $"{ViewLang.Old}: {log.OldCollStatus}\n\r{ViewLang.New}: {log.NewCollStatus}";
                    ExportExcel.RenderCurrentCell(detail, ExcelStyleTemplate.TextStyle);

                    ExportExcel.CurrentRow++;
                }
            }
            else
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
            }
        }
    }
}
