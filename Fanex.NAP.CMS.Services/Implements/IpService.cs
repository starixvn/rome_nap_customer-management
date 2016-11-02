using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.CMS.Criteria.Ips;
using Fanex.NAP.CMS.Models.Ips;
using Fanex.NAP.CMS.Services.Interfaces;

namespace Fanex.NAP.CMS.Services.Implements
{
    public class IpService : IIpService
    {
        private readonly IDynamicRepository _dynamicRepository;

        public IpService(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public IList<IpVnBean> GetAllIpVns()
        {
            IList<IpVnBean> ipVnBeans;
            try
            {
                ipVnBeans = _dynamicRepository.Fetch<IpVnBean>(new GetIpVnCriteria()).ToList();
            }
            catch (Exception)
            {
                ipVnBeans = new List<IpVnBean>();
            }

            return ipVnBeans;
        }

        public bool IsValidIp(string ipAddress)
        {
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])" + @"(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            Regex check = new Regex(pattern);
            if (string.IsNullOrEmpty(ipAddress))
            {
                return false;
            }

            return check.IsMatch(ipAddress, 0);
        }

        public bool IsVnIp(string ip)
        {
            try
            {
                var ipNumber = Ip2Number(ip);
                IList<IpVnBean> ipVnBeans = GetAllIpVns();
                int total = ipVnBeans.Count;
                for (int i = 0; i < total; i++)
                {
                    IpVnBean item = ipVnBeans[i];
                    if (item == null)
                    {
                        return false;
                    }

                    if (item.IpFrom <= ipNumber && ipNumber <= item.IpTo)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private double Ip2Number(string ip)
        {
            try
            {
                double longIp = 0;
                if (IsValidIp(ip))
                {
                    string[] sIp = ip.Split('.');
                    longIp = (double.Parse(sIp[0]) * Math.Pow(256, 3))
                        + (double.Parse(sIp[1]) * Math.Pow(256, 2))
                        + (double.Parse(sIp[2]) * 256)
                        + double.Parse(sIp[3]);
                }

                return longIp;
            }
            catch (Exception)
            {
            }

            return 0.0;
        }
    }
}