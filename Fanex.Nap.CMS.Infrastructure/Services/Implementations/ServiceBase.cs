// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs">
namespace Fanex.Nap.CMS.Infrastructure.Services.Implementations
{
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

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

        public virtual IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            return (new DynamicRepository()).Fetch<T>(criteria);
        }

        public virtual Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.AddOutParameter("errcode", System.Data.DbType.Int32, sizeof(int));
                objDb.AddOutParameter("errmsg", System.Data.DbType.String, int.MaxValue);
                objDb.ExecuteNonQuery();
                var outputs = objDb.GetOuputParams();
                return new Result(CMSConverter.ToInt32(outputs["errcode"]), CMSConverter.ToString(outputs["errmsg"]));
            }
        }

        public virtual void ExecuteNonQueryWithMap(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            if (!criteria.IsValid())
            {
                throw new InvalidOperationException("Invalid criteria");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                if (outputParams != null)
                {
                    Map(criteria, outputParams);
                }
            }
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