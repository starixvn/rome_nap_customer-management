namespace Fanex.Nap.CMS.VirtualSettingControl.Services.ManualUpdateVRAccount
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Core.Resources;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Services.Implementations;
    using Infrastructure.Services.Interfaces;
    using Models.ManualUpdateVRAccount;
    using Resources;

    public sealed class ManualUpdateVRAccountExporter : BaseExcelExporter, IExcelExporter
    {
        private const int Width120 = 120;
        private const int Width300 = 300;

        private readonly IEnumerable<ManualUpdateAccountDTO> data;

        public ManualUpdateVRAccountExporter(IEnumerable<ManualUpdateAccountDTO> data)
        {
            this.data = data;
        }

        public override string FileNameWithoutExtension
            => $"{ESportsResource.ESportsManualVRAccountsExcelFileName}_{DateTime.Today.ToString("yyyyMMdd")}";

        public override string Title
            => ESportsResource.ESports_ManuallyUpdatedVRAccounts;

        public override ExcelColumn[] ExcelColumns
            => new[]
            {
                new ExcelColumn(ViewLang.Username, Width300),
                new ExcelColumn(ViewLang.Level, Width120)
            };

        protected override void ExportBody()
        {
            var textStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Left);
            var centerTextStyle = NpoiExcelExtensions.TextStyle(ExportExcel.ExcelFile, ExcelStyleTemplate.BodyColorIndex, false, Alignment.Center);

            foreach (var item in data)
            {
                ExportExcel.ResetCurrentColumn();

                ExportExcel.RenderCurrentCell(item.Username, textStyle);
                ExportExcel.RenderCurrentCell(item.Level, centerTextStyle);

                ExportExcel.CurrentRow++;
            }
        }
    }
}
