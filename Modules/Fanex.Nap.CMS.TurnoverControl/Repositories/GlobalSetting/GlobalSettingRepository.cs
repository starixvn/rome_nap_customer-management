// --------------------------------------------------------------------------
// <copyright file="GlobalSettingRepository.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;

    /// <summary>
    /// Global Setting Repository.
    /// </summary>
    public class GlobalSettingRepository : BaseRepository, IGlobalSettingRepository
    {
        /// <summary>
        /// Gets the global setting list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The global setting list.</returns>
        public List<CustomerInfo> GlobalSettingList(GlobalSettingListCriteria criteria)
        {
            List<CustomerInfo> result = new List<CustomerInfo>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var customerInfo = new CustomerInfo
                    {
                        Id = Converter.ToInt(reader["custid"]),
                        Name = Converter.ToString2(reader["username"]),
                        RoleInfo = new IdNameModel
                        {
                            Name = Converter.ToString2(reader["level"])
                        },
                        Currency = new IdNameModel
                        {
                            Name = Converter.ToString2(reader["currency"])
                        },
                        MaxTurnover = Converter.ToDecimal(reader["maxturnover"])
                    };

                    result.Add(customerInfo);
                }
            }

            return result;
        }

        public Result EditGlobalSetting(EditGlobalSettingCriteria criteria)
        {
            return DoAction(criteria);
        }

        public Result AddGlobalSetting(AddGlobalSettingCriteria criteria)
        {
            return DoAction(criteria);
        }

        public Result DeleteGlobalSetting(DeleteGlobalSettingCriteria criteria)
        {
            return DoAction(criteria);
        }
    }
}
