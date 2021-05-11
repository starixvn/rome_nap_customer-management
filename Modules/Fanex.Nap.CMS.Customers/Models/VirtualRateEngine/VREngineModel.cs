namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    using System.Collections.Generic;

    public class VREngineModel
    {
        public VREngineModel(
            IEnumerable<VREngineRule> rules,
            VREngineMatrix matrix)
        {
            Rules = rules;
            Matrix = matrix;
        }

        public IEnumerable<VREngineRule> Rules { get; }

        public VREngineMatrix Matrix { get; }
    }
}
