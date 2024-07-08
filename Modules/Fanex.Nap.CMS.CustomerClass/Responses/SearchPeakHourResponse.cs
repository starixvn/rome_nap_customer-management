namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using System.Collections.Generic;
    
    public class SearchPeakHourResponse
    {
        public IEnumerable<SpreadRule> SpreadRules { get; set; }
        public string PeakNoteInfo { get; set; }
    }
}
