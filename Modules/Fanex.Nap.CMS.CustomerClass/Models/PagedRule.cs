namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;

    internal class PagedRule
    {
        public PagedRule(int totalItems, IEnumerable<Rule> rules, Result error)
        {
            TotalItems = totalItems;
            Rules = rules;
            Message = error.Code != 0 ? error.Message : null;
        }

        public int TotalItems { get; }

        public string Message { get; }

        public IEnumerable<Rule> Rules { get; }
    }
}
