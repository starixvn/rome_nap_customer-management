using Fanex.NAP.Client.Helpers.Web;

namespace Fanex.Nap.CMS.Infrastructure.Requests
{
    public class CommandBase
    {
        public int UserId { get; set; }

        public string IP { get; set; }

        public void AssignUserIdAndIP(INapSession session, ICurrentContext context)
            => AssignUserIdAndIP(session.UserId, context.IP);

        public void AssignUserIdAndIP(int userId, string ip)
        {
            UserId = userId;
            IP = ip;
        }

        public virtual bool IsValid() => UserId > 0;
    }
}
