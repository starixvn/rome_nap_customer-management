namespace Fanex.NAP.CMS.BetLimitControl.AppBundles
{
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using System.Web.Optimization;

    public static class BetLimitBundlesConfig
    {
        public const string BetLimitIncreaseStyles = "~/bundles/betlimit/betlimit-increase-styles";
        public const string BetLimitIncreaseScripts = "~/bundles/betlimit/betlimit-increase-scripts";

        private const string FanexMultiSelectStyle = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css";
        private const string FanexMultiSelectCustomStyle = "~/Content/MultipleSelect/fanex.multiSelect-custom.less";
        private const string FanexMultiSelectScript = "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js";

        public static void RegisterBundles(BundleCollection bundles)
            => bundles
                .RegisterBetLimitIncreaseBundles();

        private static BundleCollection RegisterBetLimitIncreaseBundles(this BundleCollection bundles)
        {
            bundles.AddAndInclude(
                BetLimitIncreaseStyles,
                FanexMultiSelectStyle,
                FanexMultiSelectCustomStyle,
                "~/Areas/BetLimitIncrease/Styles/index.less");

            bundles.Add(new ScriptBundle(BetLimitIncreaseScripts).Include(
                FanexMultiSelectScript,
                "~/Areas/BetLimitIncrease/Scripts/index.js",
                "~/Areas/BetLimitIncrease/Scripts/add-edit-betlimit.js"));

            return bundles;
        }
    }
}
