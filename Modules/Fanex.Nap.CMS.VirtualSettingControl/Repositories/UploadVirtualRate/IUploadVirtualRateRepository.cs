namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.UploadVirtualRate
{
    using Fanex.Data.Repository;
    using Infrastructure.Models;
    using Models.UploadVirtualRate;

    public interface IUploadVirtualRateRepository
    {
        ExcelErrorModel VerifyExcelTemplate(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
