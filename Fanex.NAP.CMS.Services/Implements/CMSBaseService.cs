// --------------------------------------------------------------------------
// <copyright file="CMSBaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Base Service.
    /// </summary>
    public class CMSBaseService
    {
        public const string CacheBalanceUp = "Fanex.CMS.Service.Common.BalanceUp";
        private const string CachedCountryList = "Fanex.CMS.Service.Common.CountryList";

        private readonly IDynamicRepository _repository = new DynamicRepository();

        public IServiceContext ServiceContext { get; set; }

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
        protected virtual ICommandResult UpdateData(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                db.ExecuteNonQuery(param);

                return GetCommandResult(param);
            }
        }

        /// <summary>
        /// Gets the command result.
        /// </summary>
        /// <param name="hash">The hash table.</param>
        /// <returns>CommandResult object contains error code and error message.</returns>
        private CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];

            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }
    }
}