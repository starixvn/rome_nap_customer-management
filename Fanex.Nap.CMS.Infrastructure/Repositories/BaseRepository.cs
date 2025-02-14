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
    using System.Net;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;

    public class BaseRepository : IBaseRepository
    {
        public T GetSingle<T>(CriteriaBase criteria, Func<IDataReader, T> funcMapper)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader.Read())
                    {
                        return funcMapper(reader);
                    }
                }
            }

            return default(T);
        }

        public IEnumerable<T> ExecuteReader<T>(CriteriaBase criteria, Func<IDataReader, T> funcMapper)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return funcMapper(reader);
                    }
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<T>(criteria);
            }
        }

        public Tuple<int, IEnumerable<T>> ExecuteReaderPaging<T>(CriteriaBase criteria, Func<IDataReader, T> funcMapper)
        {
            var totalItems = 0;
            IList<T> resultSet = new List<T>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        resultSet.Add(funcMapper(reader));
                    }
                }

                var totalItemsOutputValue = db.GetOutputParams()["totalitems"];
                if (totalItemsOutputValue != null && totalItemsOutputValue != DBNull.Value)
                {
                    totalItems = Convert.ToInt32(totalItemsOutputValue);
                }
            }

            return Tuple.Create<int, IEnumerable<T>>(totalItems, resultSet);
        }
        

        public Tuple<int, IEnumerable<T>, Result> ExecuteReaderPagingWithError<T>(ICriteria criteria, Func<IDataReader, T> funcMapper)
        {
            var totalItems = 0;
            IList<T> resultSet = new List<T>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        resultSet.Add(funcMapper(reader));
                    }
                }

                var totalItemsOutputValue = db.GetOutputParams()["totalitems"];
                if (totalItemsOutputValue != null && totalItemsOutputValue != DBNull.Value)
                {
                    totalItems = Convert.ToInt32(totalItemsOutputValue);
                }

                var outputParams = db.GetOutputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = Converter.ToString2(outputParams["errmsg"]);

                var result = new Result { Code = code, Message = message };

                return Tuple.Create<int, IEnumerable<T>, Result>(totalItems, resultSet, result);
            }

        }

        public Result DoAction(CommandBaseCriteria criteria, string successfullyMessage = null)
        {
            try
            {
                using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria);

                    var message = criteria.ErrCode == 0
                        ? successfullyMessage ?? criteria.ErrMsg
                        : criteria.ErrMsg;

                    return new Result { Code = criteria.ErrCode, Message = message };
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);

                return new Result((int)HttpStatusCode.BadRequest, Labels.BadRequest);
            }
            
        }
    }
}
