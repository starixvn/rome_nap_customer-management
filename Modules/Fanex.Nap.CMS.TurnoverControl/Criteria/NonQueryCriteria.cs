namespace Fanex.Nap.CMS.TurnoverControl.Criteria
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class NonQueryCriteria : CommandBaseCriteria
    {
        public bool IsGroupHeadline { get; set; }

        public short SportType { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }
    }
}
