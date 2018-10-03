namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Models.StatusLog;

    public interface IStatusLogService
    {
        StatusLogReport Logs(LogFilter filter);
    }
}
