namespace Fanex.NAP.CMS.Repositories.Interfaces
{
    using Models.StatusLog;

    public interface IStatusLogRepository
    {
        StatusLogReport Logs(LogFilter filter);
    }
}
