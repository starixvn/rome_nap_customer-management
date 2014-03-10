// --------------------------------------------------------------------------
// <copyright file="BaseService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Implements
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;
    using Fanex.NAP.Miscellany.Core.Web;
    using Fanex.NAP.Miscellany.Criteria.Common;
    using Fanex.NAP.Miscellany.Models.Common;

    /// <summary>
    /// Base Service.
    /// </summary>
    public class BaseService
    {
        protected readonly IDynamicRepository DynamicRepository;

        private const string CachedCountryList = "Fanex.Miscellany.Service.Common.CountryList";

        public BaseService(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        public Dictionary<string, string> CountryList
        {
            get
            {
                Dictionary<string, string> countryList = CacheManager.Get<Dictionary<string, string>>(CachedCountryList);

                if (countryList == null)
                {
                    var criteria = new GetCountryListCriteria();
                    IEnumerable<CountryInfo> rawCountryList = GetData<CountryInfo>(criteria);

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

        protected IDynamicRepository Repository
        {
            get
            {
                return DynamicRepository;
            }
        }

        /// <summary>
        /// Gets the list data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list data.</returns>
        protected virtual IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return DynamicRepository.Fetch<T>(criteria);
            }
            else
            {
                return new List<T>();
            }
        }

        /// <summary>
        /// Gets the list of data and total items.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <param name="totalItems">Total items.</param>
        /// <returns>The list of data.</returns>
        protected virtual IEnumerable<T> GetData<T>(ICriteria criteria, out int totalItems)
        {
            IEnumerable<T> result = null;

            Check.Argument.IsNotNull(criteria, "criteria");
            totalItems = 0;

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable outputParam = null;

                result = db.Query<T>(criteria.GetSpParams());
                outputParam = db.GetOuputParams();

                if (outputParam.ContainsKey("totalitems"))
                {
                    totalItems = (int)outputParam["totalitems"];
                }
            }

            result = result ?? new List<T>();

            return result;
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <typeparam name="T">Generic type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The data item.</returns>
        protected virtual T GetItem<T>(ICriteria criteria) where T : new()
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return DynamicRepository.Get<T>(criteria);
            }
            else
            {
                return new T();
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