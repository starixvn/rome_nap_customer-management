using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.BetLimitTypeControl.Criteria
{
    public class DeleteCriteria : CriteriaBase
    {
        public int BetLimitTypeId { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => BetLimitTypeId > 0 && UserId > 0;

        public override string GetSettingKey() => "NAP_BLControl_TypeDelete";
    }
}
