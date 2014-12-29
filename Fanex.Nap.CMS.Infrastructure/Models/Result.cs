// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Constants;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class Result
    {
        public Result()
        {
            Code = ErrorCodeConstants.NoneError;
            Message = string.Empty;
        }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }
    }
}
