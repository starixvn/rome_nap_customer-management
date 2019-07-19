namespace Fanex.NAP.CMS.Services.Implements
{
    using Criteria.CreditCollectionSummary;
    using Framework.Command;
    using Interfaces;
    using Models.CreditCollectionSummary;
    using NAP.Core.Service.Impl;
    using NAP.Core.Service.Interface;
    using Repositories.Implements;
    using Repositories.Interfaces;

    public class CreditCollectionSummaryService : CMSBaseService, ICreditCollectionSummaryService
    {
        private readonly ICreditCollectionSummaryRepository _creditCollectionSummaryRepository;

        public CreditCollectionSummaryService(IServiceContext serviceContext) :
            this(serviceContext, null)
        {
        }

        public CreditCollectionSummaryService(IServiceContext serviceContext, ICreditCollectionSummaryRepository creditCollectionSummaryRepository)
        {
            ServiceContext = serviceContext ?? new ServiceContext();
            _creditCollectionSummaryRepository = creditCollectionSummaryRepository ?? new CreditCollectionSummaryRepository();
        }

        public CreditCollectionSummaryReport Generate(CreditCollectionSummaryFilter filter)
        {
            var criteria = new CreditCollectionSummaryCriteria
            {
                Code = filter.Code,
                AccountType = filter.AccountType,
                Status = filter.Status,
                LastBalanceDate = filter.LastBalanceDate,
                CurrentBalanceDate = filter.CurrentBalanceDate,
                BalanceUpDay = filter.BalanceUpDay,
                MoveBetsDay = filter.MoveBetsDay,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };


            return _creditCollectionSummaryRepository.Generate(criteria);
        }

        public ICommandResult Save(SaveCreditStatusParam param)
        {
            var criteria = new SaveCreditStatusCriteria
            {
                CustId = param.CustId,
                Status = param.Status,
                UserID = param.UserId,
                IP = param.IP
            };

            return UpdateData(criteria);
        }
    }
}
