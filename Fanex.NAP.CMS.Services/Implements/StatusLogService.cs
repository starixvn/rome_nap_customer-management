namespace Fanex.NAP.CMS.Services.Implements
{
    using Interfaces;
    using Models.StatusLog;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using Repositories.Implements;
    using Repositories.Interfaces;

    public class StatusLogService : CMSBaseService, IStatusLogService
    {
        private readonly IStatusLogRepository _statusLogRepository;

        public StatusLogService(IServiceContext serviceContext) :
            this(serviceContext, null)
        {
        }

        public StatusLogService(IServiceContext serviceContext, IStatusLogRepository statusLogRepository)
        {
            ServiceContext = serviceContext ?? new ServiceContext();
            _statusLogRepository = statusLogRepository ?? new StatusLogRepository();
        }

        public StatusLogReport Logs(LogFilter filter)
        {
            return _statusLogRepository.Logs(filter);
        }
    }
}
