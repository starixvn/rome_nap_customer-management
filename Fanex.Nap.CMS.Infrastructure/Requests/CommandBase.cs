namespace Fanex.Nap.CMS.Infrastructure.Requests
{
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    public class CommandBase
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public void AssignUserIdAndIP(ICurrentSession session, ICurrentContext context)
            => AssignUserIdAndIP(session.UserId, context.IP);

        public void AssignUserIdAndIP(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public virtual bool IsValid() => UserId > 0;
    }
}
