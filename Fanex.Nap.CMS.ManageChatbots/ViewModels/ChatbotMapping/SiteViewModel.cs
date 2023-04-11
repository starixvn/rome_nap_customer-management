using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Fanex.Nap.CMS.ManageChatbots.ViewModels.ChatbotMapping
{
    [DataContract]
    public class SiteViewModel
    {

        public SiteViewModel()
        {
            ChildSites = new List<SiteViewModel>();
            IsParent = ChildSites.Any();
        }

        public SiteViewModel(string id, string name = null, IEnumerable<SiteViewModel> sites = null)
        {
            Id = id;
            Name = name;
            ChildSites = sites?.ToList();
            IsParent = ChildSites != null ? ChildSites.Any() : false;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<SiteViewModel> ChildSites { get; set; }

        [DataMember]
        public bool IsParent { get; set; }
    }
}
