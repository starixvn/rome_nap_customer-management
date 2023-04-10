// --------------------------------------------------------------------------
// <copyright file="BaseRepository.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;

    public class BaseRepository : IBaseRepository
    {
        public T GetSingle<T>(ICriteria criteria, Func<IDataReader, T> funcMapper)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return funcMapper(reader);
                    }
                }
            }

            return default(T);
        }

        public IEnumerable<T> ExecuteReader<T>(ICriteria criteria, Func<IDataReader, T> funcMapper)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return funcMapper(reader);
                    }
                }
            }
        }

        public Tuple<int, IEnumerable<T>> ExecuteReaderPaging<T>(ICriteria criteria, Func<IDataReader, T> funcMapper)
        {
            var totalItems = 0;
            IList<T> resultSet = new List<T>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        resultSet.Add(funcMapper(reader));
                    }
                }

                var totalItemsOutputValue = db.GetOuputParams()["totalitems"];
                if (totalItemsOutputValue != null && totalItemsOutputValue != DBNull.Value)
                {
                    totalItems = Convert.ToInt32(totalItemsOutputValue);
                }
            }

            return Tuple.Create<int, IEnumerable<T>>(totalItems, resultSet);
        }

        public Result DoAction(ICriteria criteria, string successfullyMessage = null)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = code == 0
                    ? successfullyMessage ?? Converter.ToString2(outputParams["errmsg"])
                    : Converter.ToString2(outputParams["errmsg"]);

                return new Result { Code = code, Message = message };
            }
        }
    }
}
