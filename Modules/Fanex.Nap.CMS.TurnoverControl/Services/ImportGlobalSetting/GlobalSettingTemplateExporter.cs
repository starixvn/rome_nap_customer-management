using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;

namespace Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting
{
    public sealed class GlobalSettingTemplateExporter : BaseExcelExporter, IExcelExporter
    {
        private const int Width80 = 80;
        private const int Width120 = 120;
        private const int Width160 = 160;
        private const string MaxTurnover = "1,000.00";

        private readonly IEnumerable<IdNameModel> profiles;

        public GlobalSettingTemplateExporter(IEnumerable<IdNameModel> profiles)
        {
            this.profiles = profiles;
        }

        public override string FileNameWithoutExtension { get; } = "ImportMaxTOTemplate";

        public override ExcelColumn[] ExcelColumns
            => new[]
            {
                new ExcelColumn(Labels.UserName, Width160),
                new ExcelColumn(Labels.Headline, Width80),
            }.Concat(profiles.Select(p => new ExcelColumn(p.Name, Width120))).ToArray();

        protected override void ExportBody()
        {
            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            var defaultData = GetDefaultData();

            foreach (var item in defaultData)
            {
                ExportExcel.ResetCurrentColumn();
                ExportExcel.RenderCurrentCell(item.Username, ExcelStyleTemplate.TextStyle);
                ExportExcel.RenderCurrentCell(item.Headline, centerTextStyle);

                foreach (var mto in item.MaxTurnovers)
                {
                    ExportExcel.RenderCurrentCell(mto.MaxTurnover, ExcelStyleTemplate.DecimalStyle);
                }

                ExportExcel.CurrentRow++;
            }
        }

        private GlobalSettingExcel[] GetDefaultData()
        {
            var maxTurnovers = profiles.Select(p => new MaxTurnoverExcel(p.Id, MaxTurnover));

            return new[]
            {
                new GlobalSettingExcel("super01", string.Empty, maxTurnovers),
                new GlobalSettingExcel("super02", string.Empty, maxTurnovers),
                new GlobalSettingExcel("super02", "x", maxTurnovers),
                new GlobalSettingExcel("super04", string.Empty, maxTurnovers)
            };
        }
    }
}
