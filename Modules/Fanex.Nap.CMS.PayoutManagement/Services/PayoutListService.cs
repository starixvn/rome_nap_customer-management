namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.PayoutManagement.Criteria;
    using Fanex.Nap.CMS.PayoutManagement.Enums;
    using Fanex.Nap.CMS.PayoutManagement.Models;
    using Fanex.Nap.CMS.PayoutManagement.Repositories;
    using Fanex.Nap.CMS.PayoutManagement.Resources;

    public class PayoutListService : IPayoutListService
    {
        private const string SitesLicenseesSessionKey = "PayoutService_SitesLicensees";
        private const string ProductsSessionKey = "PayoutService_Products";

        private readonly IPayoutListRepository _payoutRepository;

        public PayoutListService(IPayoutListRepository payoutRepository = null)
        {
            _payoutRepository = payoutRepository ?? new PayoutListRepository();
        }

        public void GetSitesLicenseesProducts(out IList<GroupIdNameModel> sitesLicensees, out IList<IdNameModel> products)
        {
            sitesLicensees = SessionManager.Get<IList<GroupIdNameModel>>(SitesLicenseesSessionKey);
            products = SessionManager.Get<IList<IdNameModel>>(ProductsSessionKey);
            if (sitesLicensees == null || products == null)
            {
                ICriteria criteria = new GetSitesLicenseesProductsCriteria();
                _payoutRepository.GetSitesLicenseesProducts(criteria, out sitesLicensees, out products);

                SessionManager.Set<IList<GroupIdNameModel>>(SitesLicenseesSessionKey, sitesLicensees);
                SessionManager.Set<IList<IdNameModel>>(ProductsSessionKey, products);
            }
        }

        public IList<IdNameModel> GetStatusStateMachine(int status)
        {
            if (status == 0)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = 0, Name = ColossusBetsLabels.All },
                    new IdNameModel { Id = (int)PayoutStatus.Open, Name = ColossusBetsLabels.Open },
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = ColossusBetsLabels.Disbursed },
                    new IdNameModel { Id = (int)PayoutStatus.Completed, Name = ColossusBetsLabels.Completed },
                    new IdNameModel { Id = (int)PayoutStatus.Expired, Name = ColossusBetsLabels.Expired }
                };
            }

            PayoutStatus payoutStatus = (PayoutStatus)status;
            if (payoutStatus == PayoutStatus.Open)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.Open, Name = ColossusBetsLabels.Open },
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Expired, Name = ColossusBetsLabels.Expired }
                };
            }

            if (payoutStatus == PayoutStatus.InfoVerified)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.Open, Name = ColossusBetsLabels.Open },
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = ColossusBetsLabels.Disbursed },
                };
            }

            if (payoutStatus == PayoutStatus.Disbursed)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                    new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = ColossusBetsLabels.Disbursed },
                    new IdNameModel { Id = (int)PayoutStatus.Completed, Name = ColossusBetsLabels.Completed },
                };    
            }

            if (payoutStatus == PayoutStatus.Completed)
            {
                return new List<IdNameModel> 
                {
                    new IdNameModel { Id = (int)PayoutStatus.Completed, Name = ColossusBetsLabels.Completed }
                };    
            }

            return new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.Expired, Name = ColossusBetsLabels.Expired }
            };
        }

        public IList<PayoutItem> GetPayoutList(ICriteria criteria)
        {
            return _payoutRepository.GetPayoutList(criteria);
        }

        public PayoutDetail GetPayoutDetail(ICriteria criteria, bool viewSecurityCode)
        {
            var payoutDetail = _payoutRepository.GetPayoutDetail(criteria, viewSecurityCode);
            if (payoutDetail != null && !string.IsNullOrEmpty(payoutDetail.SecurityCode))
            {
                int index = payoutDetail.SecurityCode.IndexOf(',');
                if (index >= 0)
                {
                    payoutDetail.SecurityCode = payoutDetail.SecurityCode.Substring(++index);
                }
            }

            return payoutDetail;
        }

        public Result UpdateTicketStatus(ICriteria criteria)
        {
            var result = _payoutRepository.UpdateTicketStatus(criteria);
            if (result.Code == 0)
            {
                result.Message = ColossusBetsLabels.UpdateTicketStatusSuccess;
            }

            return result;
        }
    }
}
