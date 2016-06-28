// --------------------------------------------------------------------------
// <copyright file="IdNameModel.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Models
{
    using System;

    /// <summary>
    /// Identify and Name Model.
    /// </summary>
    [Serializable]
    public class IdNameModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
