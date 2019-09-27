namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Models.PossessStatistics;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    public class PossessStatisticsExporter : BaseExcelExporter, IExcelExporter
    {
        private readonly Tuple<int, IEnumerable<PossessStatisticsModel>> data;

        public PossessStatisticsExporter(Tuple<int, IEnumerable<PossessStatisticsModel>> data)
        {
            this.data = data;
        }

        public override string FileNameWithoutExtension => PossessStatisticsResource.PossessStatistics;

        public override ExcelColumn[] ExcelColumns => new[]
        {
            new ExcelColumn(PossessStatisticsResource.Sportsbook, 100),
            new ExcelColumn(PossessStatisticsResource.NonSport, 100),
            new ExcelColumn(PossessStatisticsResource.Username, 200),
            new ExcelColumn(PossessStatisticsResource.Sportsbook, 100),
            new ExcelColumn(PossessStatisticsResource.NonSport, 100),
            new ExcelColumn(PossessStatisticsResource.Username, 200),
            new ExcelColumn(PossessStatisticsResource.Sportsbook, 100),
            new ExcelColumn(PossessStatisticsResource.NonSport, 100),
            new ExcelColumn(PossessStatisticsResource.Username, 200),
            new ExcelColumn(PossessStatisticsResource.Sportsbook, 100),
            new ExcelColumn(PossessStatisticsResource.NonSport, 100),
            new ExcelColumn(PossessStatisticsResource.Status, 100),
            new ExcelColumn(PossessStatisticsResource.ABIDangerLevel, 150),
            new ExcelColumn(PossessStatisticsResource.Monitored, 100),
        };

        protected override void ExportHeader()
        {
            ExportExcel.MaxColumnIndex = ExcelColumns.Length - 1;
            ExportExcel.SetColumnWidthByFontWidth(ExcelColumns.Select(c => c.Width).ToArray());

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.ResetCurrentColumn();

            ExportExcel.RenderCell(ExportExcel.CurrentRow, ExportExcel.CurrentRow, ExportExcel.CurrentColumn + 1, PossessStatisticsResource.TotalPercent, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(ExportExcel.CurrentRow, ExportExcel.CurrentRow, ExportExcel.CurrentColumn + 2, PossessStatisticsResource.Super, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(ExportExcel.CurrentRow, ExportExcel.CurrentRow, ExportExcel.CurrentColumn + 2, PossessStatisticsResource.Master, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.RenderCell(ExportExcel.CurrentRow, ExportExcel.CurrentRow, ExportExcel.CurrentColumn + 5, PossessStatisticsResource.Agent, ExcelStyleTemplate.HeaderTextStyle);
            ExportExcel.CurrentRow++;

            ExportExcel.CreateCurrentNPOIRow();
            ExportExcel.ResetCurrentColumn();

            foreach (ExcelColumn column in ExcelColumns)
            {
                ExportExcel.RenderCurrentCell(column.Name, ExcelStyleTemplate.HeaderTextStyle);
            }

            ExportExcel.CurrentRow++;
        }

        protected override void ExportBody()
        {
            if (data.Item1 == 0)
            {
                ExportExcel.RenderEmptyReport(NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center));
                return;
            }

            var boldIntStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, true, Alignment.Center);
            var centerIntStyle = NpoiExcelExtensions.IntStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);
            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            foreach (var item in data.Item2)
            {
                ExportExcel.ResetCurrentColumn();
                ExportExcel.RenderCurrentCell(item.TotalSB, boldIntStyle);
                ExportExcel.RenderCurrentCell(item.TotalNS, boldIntStyle);

                ExportExcel.RenderCurrentCell(item.SuperUsername, ExcelStyleTemplate.TextStyle);
                ExportExcel.RenderCurrentCell(item.SuperSB, centerIntStyle);
                ExportExcel.RenderCurrentCell(item.SuperNS, centerIntStyle);

                ExportExcel.RenderCurrentCell(item.MasterUsername, ExcelStyleTemplate.TextStyle);
                ExportExcel.RenderCurrentCell(item.MasterSB, centerIntStyle);
                ExportExcel.RenderCurrentCell(item.MasterNS, centerIntStyle);

                ExportExcel.RenderCurrentCell(item.AgentUsername, ExcelStyleTemplate.TextStyle);
                ExportExcel.RenderCurrentCell(item.AgentSB, centerIntStyle);
                ExportExcel.RenderCurrentCell(item.AgentNS, centerIntStyle);
                ExportExcel.RenderCurrentCell(item.StatusName, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.DangerLevel, centerIntStyle);
                ExportExcel.RenderCurrentCell(item.MonitoredText, centerTextStyle);

                ExportExcel.CurrentRow++;
            }
        }
    }
}