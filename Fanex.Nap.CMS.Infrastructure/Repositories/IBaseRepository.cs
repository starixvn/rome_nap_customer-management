// --------------------------------------------------------------------------
// <copyright file="IBaseRepository.cs">
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
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IBaseRepository
    {
        T GetSingle<T>(CriteriaBase criteria, Func<IDataReader, T> funcMapper);

        IEnumerable<T> ExecuteReader<T>(CriteriaBase criteria, Func<IDataReader, T> funcMapper);

        IEnumerable<T> ExecuteReader<T>(CriteriaBase criteria);

        Tuple<int, IEnumerable<T>> ExecuteReaderPaging<T>(CriteriaBase criteria, Func<IDataReader, T> funcMapper);

        Result DoAction(CommandBaseCriteria criteria, string successfullyMessage = null);
    }
}
