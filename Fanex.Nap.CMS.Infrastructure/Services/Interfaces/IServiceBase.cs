// --------------------------------------------------------------------------
// <copyright file="IServiceBase.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.6">Move</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;

    public interface IServiceBase
    {
        IEnumerable<T> GetRepositoryData<T>(ICriteria criteria);

        void ExecuteNonQueryWithMapOutput(ICriteria criteria);
    }
}
