using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Models.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;

namespace Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting
{
    public sealed class GlobalSettingTemplateExporter : BaseExcelExporter, IExcelExporter
    {
        public override string FileNameWithoutExtension
        {
            get
            {
                return "ImportMaxTOTemplate";
            }
        }

        public override ExcelColumn[] ExcelColumns
        {
            get
            {
                return new[]
                {
                    new ExcelColumn(Labels.UserName, 160),
                    new ExcelColumn(Labels.Headline, 160),
                    new ExcelColumn(Labels.Max_Turnover, 160)
                };
            }
        }

        protected override void ExportBody()
        {
            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            var defaultData = GetDefaultData();

            foreach (var item in defaultData)
            {
                ExportExcel.ResetCurrentColumn();
                ExportExcel.RenderCurrentCell(item.UserName, ExcelStyleTemplate.TextStyle);
                ExportExcel.RenderCurrentCell(item.Headline, centerTextStyle);
                ExportExcel.RenderCurrentCell(item.MaxTurnover, ExcelStyleTemplate.DecimalStyle);
                ExportExcel.CurrentRow++;
            }
        }

        private static GlobalSettingExcel[] GetDefaultData()
        {
            return new[] 
            {
                new GlobalSettingExcel("super01", string.Empty, "1,000.00"),
                new GlobalSettingExcel("super02", string.Empty, "1,000.00"),
                new GlobalSettingExcel("super02", "x", "1,000.00"),
                new GlobalSettingExcel("super04", string.Empty, "1,000.00")
            };
        }
    }
}
