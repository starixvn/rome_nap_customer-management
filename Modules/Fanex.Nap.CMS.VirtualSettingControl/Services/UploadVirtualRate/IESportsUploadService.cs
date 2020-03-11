namespace Fanex.Nap.CMS.VirtualSettingControl.Services.UploadVirtualRate
{
    using System.Collections.Generic;
    using System.IO;
    using Infrastructure.Models;
    using Models.UploadVirtualRate;

    public interface IESportsUploadService
    {
        IEnumerable<ExcelErrorDTO> Verify(Stream stream, string fileExtension, int userId);

        Result Update(int userId, string ip);
    }
}
