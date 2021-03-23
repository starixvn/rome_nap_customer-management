using System;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Extensions;

namespace Fanex.Nap.CMS.Customers.Models.LimitNewDownline
{
    public class LimitDownlineItem
    {
        public string Username { get; set; }

        public byte RoleId { get; set; }

        public string Level => RoleIdConstants.GetRoleName(RoleId);

        public string Currency { get; set; }

        public string Site { get; set; }

        public string AddedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string AddedDate => CreatedDate.ToLongDate24hWithSlash();
    }
}
