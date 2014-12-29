// --------------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Resources;

    /// <summary>
    /// Customer Information.
    /// </summary>
    [Serializable]
    public class CustomerInfo : IdNameModel
    {
        public IdNameModel RoleInfo { get; set; }

        public IdNameModel Currency { get; set; }

        public decimal MaxTurnover { get; set; }

        public decimal PercentageUsed { get; set; }

        /// <summary>
        /// Gets the name of the role.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>Role name.</returns>
        public static string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case RoleIdConstants.Super:
                    return Labels.Super;
                case RoleIdConstants.Master:
                    return Labels.Master;
                case RoleIdConstants.Agent:
                    return Labels.Agent;
                case RoleIdConstants.Member:
                    return Labels.Member;
                default:
                    return string.Empty;
            }
        }
    }
}
