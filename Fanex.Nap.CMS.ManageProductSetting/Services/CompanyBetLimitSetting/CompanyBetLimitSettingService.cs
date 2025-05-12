namespace Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using System.Linq;
    using Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting;
    using Fanex.Data.Repository;
    using System.Text;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using NPOI.XSSF.UserModel;
    using Fanex.Logging.Extensions.Web;
    using NPOI.SS.UserModel;
    using System.Collections;
    using System.Web;
    using Fanex.Nap.CMS.ManageProductSetting.Resources;
    using System.Web.Mvc;

    public interface ICompanyBetLimitSettingService
    {
        IEnumerable<IdNameModel> GetCurrencies(bool isCredit);
        
        IEnumerable<IdNameModel> GetProducts(bool isCredit);
        
        IEnumerable<IdNameModel> GetSites(bool isCredit, bool isGetAll);

        GetCurrencyGroupViewModel GetCurrencyGroups(bool isCredit);

        IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(GetCompanyBetLimitSettingViewModels searchParams);

        IEnumerable<CompanyBetLimitSettingGroups> GetBetLimitSettingGroups(IEnumerable<CompanyBetLimitSetting> betLimitSetting);

        Result UpdateBetLimitSetting(IEnumerable<UpdateBetLimitSettingModel> settings, int userId, string ip);

        Result DeleteBetLimitSetting(IEnumerable<CompanyBetLimitSetting> settings, bool isCredit, int userId, string iP);

        ValidateBetLimitSettingResponse VerifyByWeb(Stream inputStream, string fileExtension, int userId, string iP);

        Result ImportExcel(IEnumerable<CompanyBetLimitSettingExcel> request, int userId, string iP);

        Result InsertCurrencyGroups(CurrencyGroup currencyGroup, bool isCredit, int userId, string iP);

        Result UpdateCurrencyGroups(CurrencyGroup currencyGroup, bool isCredit, int userId, string iP);

        Result DeleteCurrencyGroups(int currencyGroupId, bool isCredit, int userId, string iP);
    }

    public class CompanyBetLimitSettingService : ICompanyBetLimitSettingService
    {
        private const string CurrencyCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Currency";

        private const string ProductCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Product";

        private const string SiteCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.Site";

        private const string CurrencyGroupCacheKey =
            "Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting.CurrencyGroup";

        private const string CommarChar = ",";

        private readonly IDynamicRepository dynamicRepository;

        public CompanyBetLimitSettingService() : this(new DynamicRepository())
        {
        }

        public CompanyBetLimitSettingService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        private IEnumerable<T> GetCachedData<T>(string cacheKey, Func<IEnumerable<T>> getRepositoryData)
        {
            var data = CacheManager.Get<List<T>>(cacheKey);

            if (data != null) return data;
            data = getRepositoryData().ToList();

            CacheManager.Set(cacheKey, data, new TimeSpan(0, 0, 5, 0));

            return data;
        }

        public IEnumerable<IdNameModel> GetCurrencies(bool isCredit) =>
            GetCachedData(CurrencyCacheKey + isCredit,
                () => dynamicRepository.Fetch<IdNameModel>(new GetCurrenciesCriteria { IsCredit = isCredit }));

        public IEnumerable<IdNameModel> GetProducts(bool isCredit) => GetCachedData(
            ProductCacheKey + isCredit, () => dynamicRepository.Fetch<IdNameModel>(new GetProductsCriteria { IsCredit = isCredit }));

        public IEnumerable<IdNameModel> GetSites(bool isCredit, bool isGetAll) => GetCachedData(SiteCacheKey + (isGetAll ? "All" : isCredit.ToString()),
            () => dynamicRepository.Fetch<IdNameModel>(new GetSiteCriteria { IsCredit = isCredit }));

        public GetCurrencyGroupViewModel GetCurrencyGroups(bool isCredit)
        {
            var result = new GetCurrencyGroupViewModel { IsCredit = isCredit };
            var currencyGroups = dynamicRepository.Fetch<CurrencyGroupItem>(new GetCurrencyGroupCriteria { IsCredit = isCredit });
            result.Currencies = currencyGroups.Select(e => new SelectListItem { Text = string.Format("[{0}] {1}", e.ExchangeID, e.Currency), Value = e.ExchangeID.ToString() });
            result.CurrencyGroups = currencyGroups.Where(cur => cur.CurrencyGroupID != null).GroupBy(cur => (int)cur.CurrencyGroupID).Select(curGroup => new CurrencyGroup(curGroup.Key, curGroup.ToList()));
            
            return result;
        }

        public IEnumerable<CompanyBetLimitSetting> GetBetLimitSetting(GetCompanyBetLimitSettingViewModels searchParams)
        {
            if (string.IsNullOrWhiteSpace(searchParams.ProductIds) ||
                string.IsNullOrWhiteSpace(searchParams.Currencies) ||
                string.IsNullOrWhiteSpace(searchParams.Sites))
            {
                return new List<CompanyBetLimitSetting>();
            }

            var currencies = GetCurrencies(searchParams.IsCredit);
            var sites = GetSites(searchParams.IsCredit, false);

            return dynamicRepository.Fetch<CompanyBetLimitSetting>(new GetBetLimitSettingCriteria
            {
                IsCredit = searchParams.IsCredit,
                PMutilProductID = searchParams.ProductIds,
                PMutilCurrency = searchParams.Currencies,
                PMutilSite = searchParams.Sites,
            }).Select(setting => {
                setting.Currency = currencies.FirstOrDefault(cur => cur.Id == setting.ExchangeID)?.Name;
                setting.Site = 0 == setting.SiteID ? "All Sites" : sites.FirstOrDefault(site => site.Id == setting.SiteID)?.Name;

                return setting;
            });
        }

        public IEnumerable<CompanyBetLimitSettingGroups> GetBetLimitSettingGroups(
            IEnumerable<CompanyBetLimitSetting> betLimitSetting)
        {
            return betLimitSetting
                .OrderBy(entry => entry.SportType)
                .GroupBy(entry1 => entry1.SportType)
                .Select(group1 => new CompanyBetLimitSettingGroups()
                {
                    SportType = group1.Key,
                    SportName = group1.First().SportName,
                    CurrencyGroups = group1.GroupBy(entry2 => entry2.CurrencyGroupID)
                        .Select(group2 => new BetLimitCurrencyGroup
                        {
                            CurrencyGroupID = group2.Key,
                            Currency = string.Join(", ", group2.Select(e => e.Currency).Distinct()),
                            SiteGroups = group2.GroupBy(entry3 => entry3.Site)
                                .Select(group3 => new Models.CompanyBetLimitSetting.SiteGroup
                                {
                                    SiteGroupName = group3.Key,
                                    GameGroups = group3.GroupBy(entry4 => entry4.GameGroup)
                                        .Select(group4 => new GameGroup
                                        {
                                            GameGroupName = group4.Key,
                                            Entries = group3.ToList()
                                        }).ToList()
                                }).ToList()
                        })
                        .ToList()
                })
                .ToList();
        }

        public Result UpdateBetLimitSetting(IEnumerable<UpdateBetLimitSettingModel> settings, int userId, string ip)
        {

            var criteriaCheck = new CheckBetLimitSettingCriteria
            {
                InputData = BuildInsertUpdateXML(settings)
            };

            var resultCheck = DoAction(criteriaCheck);
            if (!resultCheck.IsSuccess)
            {
                return new Result(resultCheck.Code, resultCheck.Message);
            }

            var updateCriteria = new InsertBetLimitSettingCriteria
            {
                UserID = userId,
                IP = ip,
                InputData = criteriaCheck.InputData
            };

            return DoAction(updateCriteria);
        }

        public Result DeleteBetLimitSetting(IEnumerable<CompanyBetLimitSetting> settings, bool isCredit, int userId, string iP)
        {
            var deleteCriteria = new DeleteBetLimitSettingCriteria
            {
                IsCredit = isCredit,
                UserID = userId,
                IP = iP,
                InputData = BuildDeleteXML(settings)
            };

            return DoAction(deleteCriteria);
        }

        private string BuildInsertUpdateXML(IEnumerable<UpdateBetLimitSettingModel> settings)
        {
            var betLimitXml = new StringBuilder();
            foreach (var setting in settings)
            {
                if (IsHaveChangeData(setting))
                {
                    betLimitXml.Append("<BetLimit");
                    betLimitXml.AppendFormat(" SportType=\"{0}\"", setting.SportType);
                    betLimitXml.AppendFormat(" CreditGroupID=\"{0}\"", setting.CreditGroupID ?? string.Empty);
                    betLimitXml.AppendFormat(" LicenseeGroupID=\"{0}\"", setting.LicenseeGroupID ?? string.Empty);
                    betLimitXml.AppendFormat(" SiteID=\"{0}\"", setting.SiteID);
                    betLimitXml.AppendFormat(" GameGroupID=\"{0}\"", setting.GameGroupID);
                    betLimitXml.AppendFormat(" NewMinBet=\"{0}\"", setting.NewMinBet);
                    betLimitXml.AppendFormat(" NewMaxBet=\"{0}\"", setting.NewMaxBet);
                    betLimitXml.AppendFormat(" NewRemark=\"{0}\"", setting.NewRemark ?? string.Empty);
                    betLimitXml.AppendFormat(" CurMinBet=\"{0}\"", setting.CurMinBet ?? string.Empty);
                    betLimitXml.AppendFormat(" CurMaxBet=\"{0}\"", setting.CurMaxBet ?? string.Empty);
                    betLimitXml.AppendFormat(" CurRemark=\"{0}\"", setting.CurRemark ?? string.Empty);
                    betLimitXml.Append(" />");
                }
            }

            if (betLimitXml.Length == 0)
            {
                return string.Empty;
            }

            return string.Format("<ROOT>{0}</ROOT>", betLimitXml.ToString());
        }

        private bool IsHaveChangeData(UpdateBetLimitSettingModel model)
        {
            return  decimal.Parse(model.NewMinBet) != decimal.Parse(model.CurMinBet) ||
                    decimal.Parse(model.NewMaxBet) != decimal.Parse(model.CurMaxBet) ||
                    model.NewRemark != model.CurRemark;
        }

        private string BuildDeleteXML(IEnumerable<CompanyBetLimitSetting> settings)
        {
            var betLimitXml = new StringBuilder();
            foreach (var setting in settings)
            {
                betLimitXml.Append("<BetLimit");
                betLimitXml.AppendFormat(" SportType=\"{0}\"", setting.SportType);
                betLimitXml.AppendFormat(" CurrencyGroupID=\"{0}\"", setting.CurrencyGroupID);
                betLimitXml.AppendFormat(" SiteID=\"{0}\"", setting.SiteID);
                betLimitXml.AppendFormat(" GameGroupID=\"{0}\"", setting.GameGroupID);
                betLimitXml.Append(" />");
            }

            return string.Format("<ROOT>{0}</ROOT>", betLimitXml.ToString());
        }

        public ValidateBetLimitSettingResponse VerifyByWeb(Stream inputStream, string fileExtension, int userId, string iP)
        {
            var workbook = GetWorkbook(inputStream, fileExtension);

            if (workbook == null)
            {
                return new ValidateBetLimitSettingResponse
                {
                    Error = new Result(-1, Labels.Error_NotExcelFile)
                };
            }

            var resultParseExcel = ParseBetLimitSetting(workbook);
            if(resultParseExcel.Error != null)
            {
                return resultParseExcel;
            }

            var criteriaCheck = new CheckBetLimitSettingCriteria
            {
                InputData = BuildCheckXML(resultParseExcel.BetLimits)
            };

            var resultCheck = DoAction(criteriaCheck);
            if (!resultCheck.IsSuccess)
            {
                return new ValidateBetLimitSettingResponse
                {
                    Error = resultCheck
                };
            }

            var creditCurrencyGroups = GetCurrencyGroups(true);
            var depositCurrencyGroups = GetCurrencyGroups(false);
            var sites = GetSites(true, true);

            return new ValidateBetLimitSettingResponse
            {
                BetLimits = resultParseExcel.BetLimits.Select(betlimit => MappingDisplayValue(betlimit, creditCurrencyGroups.CurrencyGroups, depositCurrencyGroups.CurrencyGroups, sites)).ToList()
            };
        }

        private static IWorkbook GetWorkbook(Stream stream, string fileExtension)
        {
            try
            {
                if (fileExtension.Equals(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    return new HSSFWorkbook(stream);
                }

                if (fileExtension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new XSSFWorkbook(stream);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return null;
        }

        private ValidateBetLimitSettingResponse ParseBetLimitSetting(IWorkbook workbook)
        {
            var betLimits = new List<CompanyBetLimitSettingExcel>();
            int rowCount = 1;
            try
            {
                ISheet sheet = workbook.GetSheetAt(0);
                IEnumerator rows = sheet.GetRowEnumerator();
                rows.MoveNext();
                while (rows.MoveNext())
                {
                    IRow row = (IRow)rows.Current;
                    if (!IsHaveDataRow(row))
                    {
                        continue;
                    }

                    var checkRequiredFieldsResult = CheckRequiredField(row, rowCount);
                    if (checkRequiredFieldsResult != null)
                    {
                        return new ValidateBetLimitSettingResponse
                        {
                            Error = checkRequiredFieldsResult
                        };
                    }

                    var betLimit = new CompanyBetLimitSettingExcel
                    {
                        SportTypeId = int.Parse(row.GetCell(0).ToString()),
                        SiteId = int.Parse(row.GetCell(1).ToString()),
                        CreditCurrencyGroupId = row.GetCell(2) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(2).ToString().Replace(" ", string.Empty)),
                        DepositCurrencyGroupId = row.GetCell(3) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(3).ToString().Replace(" ", string.Empty)),
                        GameGroup = row.GetCell(4) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(4).ToString().Replace(" ", string.Empty)),
                        MinBet = row.GetCell(5) == null || string.IsNullOrWhiteSpace(row.GetCell(5).ToString()) ? (decimal?)null : decimal.Parse(row.GetCell(5).ToString()),
                        MaxBet = row.GetCell(6) == null || string.IsNullOrWhiteSpace(row.GetCell(6).ToString()) ? (decimal?)null : decimal.Parse(row.GetCell(6).ToString()),
                        Remark = row.GetCell(7) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(7).ToString().Replace(" ", string.Empty)),
                        CurMinBet = row.GetCell(8) == null || string.IsNullOrWhiteSpace(row.GetCell(8).ToString()) ? (decimal?)null : decimal.Parse(row.GetCell(8).ToString()),
                        CurMaxBet = row.GetCell(9) == null || string.IsNullOrWhiteSpace(row.GetCell(9).ToString()) ? (decimal?)null : decimal.Parse(row.GetCell(9).ToString()),
                        CurRemark = row.GetCell(10) == null ? string.Empty : HttpUtility.HtmlEncode(row.GetCell(10).ToString().Replace(" ", string.Empty))
                    };

                    betLimit.SportType = CacheRef.GetSportName(betLimit.SportTypeId);
                    

                    betLimits.Add(betLimit);
                    rowCount++;
                }

                return new ValidateBetLimitSettingResponse
                {
                    BetLimits = betLimits
                };
            }
            catch (Exception ex)
            {
                var result = new Result(1, "Invalid file uploaded. Error at row: " + rowCount.ToString() + ". " + ex.Message);

                return new ValidateBetLimitSettingResponse
                {
                    Error = result
                };
            }
        }

        private bool IsHaveDataRow(IRow row)
        {
            for (int i = 0; i <= 12; i++)
            {
                if (row.GetCell(i) != null && !string.IsNullOrWhiteSpace(row.GetCell(i).ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private Result CheckRequiredField(IRow row, int rowCount)
        {
            if (row.GetCell(0) == null || string.IsNullOrWhiteSpace(row.GetCell(0).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Sport Type is required field.");
            }

            if (row.GetCell(1) == null || string.IsNullOrWhiteSpace(row.GetCell(1).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Site is required field.");
            }

            if ((row.GetCell(2) == null || string.IsNullOrWhiteSpace(row.GetCell(2).ToString()))
                && (row.GetCell(3) == null || string.IsNullOrWhiteSpace(row.GetCell(3).ToString())))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Currency is required field.");
            }

            if (row.GetCell(4) == null || string.IsNullOrWhiteSpace(row.GetCell(4).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The Game Group is required field.");
            }

            if (row.GetCell(5) == null || string.IsNullOrWhiteSpace(row.GetCell(5).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The New Min Bet is required field.");
            }

            if (row.GetCell(6) == null || string.IsNullOrWhiteSpace(row.GetCell(6).ToString()))
            {
                return new Result(1, "Row " + rowCount.ToString() + ": The New Max Bet is required field.");
            }

            return null;
        }

        private string BuildCheckXML(List<CompanyBetLimitSettingExcel> companyBetLimits)
        {
            var betLimitXml = new StringBuilder();
            foreach (var setting in companyBetLimits)
            {
                betLimitXml.Append("<BetLimit");
                betLimitXml.AppendFormat(" SportType=\"{0}\"", setting.SportTypeId);
                betLimitXml.AppendFormat(" CreditGroupID=\"{0}\"", setting.CreditCurrencyGroupId);
                betLimitXml.AppendFormat(" LicenseeGroupID=\"{0}\"", setting.DepositCurrencyGroupId);
                betLimitXml.AppendFormat(" SiteID=\"{0}\"", setting.SiteId);
                betLimitXml.AppendFormat(" GameGroupID=\"{0}\"", setting.GameGroup);
                betLimitXml.AppendFormat(" NewMinBet=\"{0}\"", setting.MinBet);
                betLimitXml.AppendFormat(" NewMaxBet=\"{0}\"", setting.MaxBet);
                betLimitXml.AppendFormat(" NewRemark=\"{0}\"", setting.Remark);
                betLimitXml.AppendFormat(" CurMinBet=\"{0}\"", setting.CurMinBet);
                betLimitXml.AppendFormat(" CurMaxBet=\"{0}\"", setting.CurMaxBet);
                betLimitXml.AppendFormat(" CurRemark=\"{0}\"", setting.CurRemark);
                betLimitXml.Append(" />");
            }

            return string.Format("<ROOT>{0}</ROOT>", betLimitXml.ToString());
        }

        private CompanyBetLimitSettingExcel MappingDisplayValue(CompanyBetLimitSettingExcel setting, IEnumerable<CurrencyGroup> creditCurGroups, IEnumerable<CurrencyGroup> depositCurGroups, IEnumerable<IdNameModel> sites)
        {
            setting.SportType = CacheRef.GetSportName(setting.SportTypeId);
            setting.CreditCurrencyGroup = string.IsNullOrWhiteSpace(setting.CreditCurrencyGroupId) ? string.Empty: creditCurGroups.FirstOrDefault(cur => cur.CurrencyGroupID.ToString() == setting.CreditCurrencyGroupId)?.CurrencyNames;
            setting.DepositCurrencyGroup = string.IsNullOrWhiteSpace(setting.DepositCurrencyGroupId) ? string.Empty : depositCurGroups.FirstOrDefault(cur => cur.CurrencyGroupID.ToString() == setting.DepositCurrencyGroupId)?.CurrencyNames;
            setting.Site = sites.FirstOrDefault(site => site.Id == setting.SiteId)?.Name;

            return setting;
        }
        public Result InsertCurrencyGroups(CurrencyGroup currencyGroup, bool isCredit, int userId, string iP)
        {
            var criteria = new InsertCurrencyGroupCriteria
            {
                UserID = userId,
                IP = iP,
                CurrencyGroup = BuildCurrencyGroupXML(currencyGroup, isCredit, false)
            };

            return DoAction(criteria);
        }

        public Result UpdateCurrencyGroups(CurrencyGroup currencyGroup, bool isCredit, int userId, string iP)
        {
            var criteria = new UpdateCurrencyGroupCriteria
            {
                UserID = userId,
                IP = iP,
                CurrencyGroup = BuildCurrencyGroupXML(currencyGroup, isCredit, true)
            };

            return DoAction(criteria);
        }

        public Result DeleteCurrencyGroups(int currencyGroupId, bool isCredit, int userId, string iP)
        {
            var criteria = new DeleteCurrencyGroupCriteria
            {
                IsCredit = isCredit,
                UserID = userId,
                IP = iP,
                CurrencyGroupID = currencyGroupId
            };

            return DoAction(criteria);
        }

        private string BuildCurrencyGroupXML(CurrencyGroup currencyGroup, bool isCredit, bool isUpdate)
        {
            var groupXml = new StringBuilder();
            groupXml.Append("<CurrencyGroup");
            groupXml.AppendFormat(" CurrencyGroupID=\"{0}\"", isUpdate ? currencyGroup.CurrencyGroupID.ToString() : string.Empty);
            groupXml.AppendFormat(" ExchangeIDs=\"{0}\"", currencyGroup.ExchangeIDs);
            groupXml.AppendFormat(" IsCredit=\"{0}\"", isCredit);
            groupXml.Append(" />");

            return string.Format("<ROOT>{0}</ROOT>", groupXml.ToString());
        }

        public Result ImportExcel(IEnumerable<CompanyBetLimitSettingExcel> request, int userId, string iP)
        {
            var criteria = new InsertBetLimitSettingCriteria
            {
                UserID = userId,
                IP = iP,
                InputData = BuildCheckXML(request.ToList())
            };

            return DoAction(criteria);
        }

        private Result DoAction(ResultQueryCommand command)
        {
            dynamicRepository.Execute(command);

            return new Result(command.ErrCode, command.ErrMsg);
        }
    }
}