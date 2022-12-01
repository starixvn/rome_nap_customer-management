namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CommandCriteriaBase : QueryCriteriaBase
    {
        protected CommandCriteriaBase(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; }

        public string ErrMsg { get; }
    }
}
