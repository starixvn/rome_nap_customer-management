namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.UploadVirtualRate
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.UploadVirtualRate;
    using Infrastructure.Models;
    using Models.UploadVirtualRate;

    public interface IUploadVirtualRateRepository
    {
        ExcelErrorModel VerifyExcelTemplate(VerifyVirtualRateExcelTemplateCriteria criteria);

        Result ExecuteNonQuery(CommandBaseCriteria criteria);
    }
}
