using Fanex.Nap.CMS.Infrastructure.Extensions;
using System.Data;

namespace Fanex.Nap.CMS.BetRestriction.Models
{
    public class CheckAddAccountResultModel
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public bool IsUplineExists { get; set; }

        public string UplineLevel { get; set; }

        public string ExistingUplineUserName { get; set; }

        public bool IsDownlineExists { get; set; }

        public short SportType { get; set; }
    }
}
