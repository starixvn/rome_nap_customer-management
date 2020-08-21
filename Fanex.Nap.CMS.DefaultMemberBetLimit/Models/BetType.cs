namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Models
{
    public class BetType
    {
        public int ProductId { get; set; }

        public int SportTypeId { get; set; }

        public int BetTypeId { get; private set; }

        public string BetTypeName { get; private set; }
    }
}
