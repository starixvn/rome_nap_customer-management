using System.Collections.Generic;
using Fanex.NAP.CMS.Models.Ips;

namespace Fanex.NAP.CMS.Services.Interfaces
{
    public interface IIpService
    {
        IList<IpVnBean> GetAllIpVns();

        bool IsValidIp(string ipAddress);

        bool IsVnIp(string ip);
    }
}
