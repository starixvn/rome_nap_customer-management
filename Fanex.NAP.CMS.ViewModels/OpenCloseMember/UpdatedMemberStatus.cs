// --------------------------------------------------------------------------
// <copyright file="UpdatedMemberStatus.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.OpenCloseMember
{
    using System;

    /// <summary>
    /// View model for updated status of a member.
    /// </summary>
    [Serializable]
    public class UpdatedMemberStatus : IEquatable<UpdatedMemberStatus>
    {
        public int CustId { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// True if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(UpdatedMemberStatus other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.CustId == other.CustId;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.CustId.GetHashCode();
        }
    }
}