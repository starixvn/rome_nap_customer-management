namespace Fanex.NAP.CMS.Services.Implements
{
    using Interfaces;
    using Models.StatusLog;
    using Repositories.Implements;
    using Repositories.Interfaces;

    public class StatusLogService : CMSBaseService, IStatusLogService
    {
        private readonly IStatusLogRepository _statusLogRepository;

        public StatusLogService() : this( null)
        {
        }

        public StatusLogService(IStatusLogRepository statusLogRepository)
        {
            _statusLogRepository = statusLogRepository ?? new StatusLogRepository();
        }

        public StatusLogReport Logs(LogFilter filter)
        {
            return _statusLogRepository.Logs(filter);
        }
    }
}
