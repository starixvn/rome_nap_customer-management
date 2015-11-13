namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    [Serializable]
    public class GroupIdNameModel
    {
        public string GroupName { get; set; }

        public IList<IdNameModel> ChildItems { get; set; }
    }
}
