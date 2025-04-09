// --------------------------------------------------------------------------
// <copyright file="CMSBaseService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

using System;
using System.Collections.Generic;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Infrastructure.Criteria;
using Fanex.NAP.CMS.Core.Web;
using Fanex.NAP.CMS.Criteria.Common;
using Fanex.NAP.CMS.Models.Common;

namespace Fanex.NAP.CMS.Services.Implements
{
    /// <summary>
    /// Base Service.
    /// </summary>
    public class CMSBaseService
    {
        public const string CacheBalanceUp = "Fanex.CMS.Service.Common.BalanceUp";
        private const string CachedCountryList = "Fanex.CMS.Service.Common.CountryList";

        private readonly IDynamicRepository _repository = new DynamicRepository();

        public IDynamicRepository Repository
        {
            get
            {
                return this._repository;
            }
        }

        public BalanceUpDayInfo BalanceUpDate
        {
            get
            {
                BalanceUpDayInfo balanceUp = CacheManager.Get<BalanceUpDayInfo>(CacheBalanceUp);

                if (balanceUp == null)
                {
                    balanceUp = this.Repository.Get<BalanceUpDayInfo>(new GetBalanceUpDayCriteria());
                    CacheManager.Set<BalanceUpDayInfo>(CacheBalanceUp, balanceUp, new TimeSpan(0, 0, 5, 0));
                }

                return balanceUp;
            }
        }

        public Dictionary<string, string> CountryList
        {
            get
            {
                Dictionary<string, string> countryList = CacheManager.Get<Dictionary<string, string>>(CachedCountryList);

                if (countryList == null)
                {
                    var criteria = new GetCountryListCriteria();
                    IEnumerable<CountryInfo> rawCountryList = this.Repository.Fetch<CountryInfo>(criteria);

                    countryList = new Dictionary<string, string>();

                    foreach (CountryInfo country in rawCountryList)
                    {
                        countryList.Add(country.CountryCode, country.CountryName);
                    }

                    CacheManager.Set<Dictionary<string, string>>(CachedCountryList, countryList, new TimeSpan(30, 0, 0, 0));
                }

                return countryList;
            }
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The query result.</returns>
        protected virtual ICommandResult UpdateData(CommandBaseCriteria criteria)
        {
            _repository.ExecuteAsync(criteria);

            return new CommandResult
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }
    }
}