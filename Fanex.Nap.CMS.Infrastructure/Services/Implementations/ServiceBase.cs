// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs">
namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    public class ServiceBase : IServiceBase
    {
        public ServiceBase() : this(null, null)
        {
        }

        public ServiceBase(IDynamicRepository repository, ICMSConverter converter)
        {
            DynamicRepository = repository ?? new DynamicRepository();
            CMSConverter = converter ?? new CMSConverterImplement();
        }

        protected IDynamicRepository DynamicRepository { get; set; }

        protected ICMSConverter CMSConverter { get; set; }

        protected IDynamicRepository Repository
        {
            get { return DynamicRepository; }
        }

        [SuppressMessage("Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public virtual IEnumerable<T> GetRepositoryData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (criteria.IsValid())
            {
                return DynamicRepository.Fetch<T>(criteria);
            }

            return new List<T>();
        }

        public virtual IEnumerable<T> GetRepositoryDataWithMapOutput<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (!criteria.IsValid())
            {
                return Enumerable.Empty<T>();
            }
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var result = db.Query<T>(criteria);
                var outputParams = db.GetOutputParams();
                if (outputParams != null)
                {
                    Map(criteria, outputParams);
                }

                return result;
            }
        }

        public virtual IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            return (new DynamicRepository()).Fetch<T>(criteria);
        }

        public virtual Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var objDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                objDb.ExecuteNonQuery(criteria);
                var outputs = objDb.GetOutputParams();
                return new Result(CMSConverter.ToInt32(outputs["errcode"]), CMSConverter.ToString(outputs["errmsg"]));
            }
        }

        public virtual void ExecuteNonQueryWithMapOutput(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (!criteria.IsValid())
            {
                throw new InvalidOperationException("Invalid criteria");
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
                var outputParams = db.GetOutputParams();
                if (outputParams != null)
                {
                    Map(criteria, outputParams);
                }
            }
        }
        public Result DoAction(CommandBaseCriteria criteria, string successfullyMessage = null)
        {
            DynamicRepository.Execute(criteria);
            var message = criteria.ErrCode == 0
                        ? successfullyMessage ?? criteria.ErrMsg
                        : criteria.ErrMsg;

            return new Result { Code = criteria.ErrCode, Message = message };
        }

        private static void Map(ICriteria criteria, Hashtable outputParams)
        {
            foreach (string key in outputParams.Keys)
            {
                var propertyInfos = criteria.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        propertyInfo.SetValue(criteria, outputParams[key]);
                    }
                }
            }
        }
    }
}