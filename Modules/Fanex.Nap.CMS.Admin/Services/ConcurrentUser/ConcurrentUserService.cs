using Fanex.Nap.CMS.Admin.Criteria;
using Fanex.Nap.CMS.Admin.Repositories;
using Fanex.Nap.CMS.Admin.ViewModels.ConcurrentUser;
using System;

namespace Fanex.Nap.CMS.Admin.Services.ConcurrentUser
{
    public interface IConcurrentUserService
    {
        ConcurrentUserViewModel GetDataLatest();
        ConcurrentUserHistoryViewModel GetDataHistory(DateTime fromDate, DateTime toDate);
    }

    public class ConcurrentUserService : IConcurrentUserService
    {
        private readonly ConcurrentUserRepository _repository;

        public ConcurrentUserService(ConcurrentUserRepository repository = null)
        {
            _repository = repository ?? new ConcurrentUserRepository();
        }

        public ConcurrentUserHistoryViewModel GetDataHistory(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetConcurrentUserCriteria(fromDate, toDate);

            var viewModel = _repository.GetDataHistory(criteria);
            return viewModel;
        }

        public ConcurrentUserViewModel GetDataLatest()
        {
            var criteria = new GetConcurrentUserCriteria();

            return _repository.GetDataLatest(criteria);
        }
    }
}
