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
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;

    public interface IBaseRepository
    {
        T GetSingle<T>(ICriteria criteria, Func<IDataReader, T> funcMapper);

        IEnumerable<T> ExecuteReader<T>(ICriteria criteria, Func<IDataReader, T> funcMapper);

        Tuple<int, IEnumerable<T>> ExecuteReaderPaging<T>(ICriteria criteria, Func<IDataReader, T> funcMapper);

        Result DoAction(ICriteria criteria, string successfullyMessage = null);
    }
}
