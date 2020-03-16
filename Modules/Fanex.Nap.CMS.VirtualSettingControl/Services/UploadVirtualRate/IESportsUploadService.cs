namespace Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate
{
    using System.IO;
    using Infrastructure.Models;
    using Models.UploadVirtualRate;

    public interface IESportsUploadService
    {
        ExcelErrorModel Verify(Stream stream, string fileExtension, int userId);

        Result Update(int userId, string ip);
    }
}
