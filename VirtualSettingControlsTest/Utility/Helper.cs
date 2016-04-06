using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VirtualSettingControlsTest.Utility
{
    public static class Helper
    {
        public static T CastJsonResult<T>(JsonResult jsonResult) where T : class
        {
            return jsonResult.Data as T;
        }
    }
}
