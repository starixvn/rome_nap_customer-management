namespace Fanex.Nap.CMS.Customers.Params
{
    public class LogParam
    {
        public LogParam(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public int UserId { get; }

        public string IP { get; }
    }
}