namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System.Collections.Generic;

    internal class PagedRule
    {
        public PagedRule(int totalItems, IEnumerable<Rule> rules)
        {
            TotalItems = totalItems;
            Rules = rules;
        }

        public int TotalItems { get; }

        public IEnumerable<Rule> Rules { get; }
    }
}
