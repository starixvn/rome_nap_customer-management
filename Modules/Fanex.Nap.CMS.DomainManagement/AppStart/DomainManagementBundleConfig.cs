namespace Fanex.Nap.CMS.DomainManagement
{
    using System.Web.Optimization;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public static class DomainManagementBundleConfig
    {
        public const string DomainManagementStyles = "~/bundles/domainmanagement/index-styles";
        public const string DomainManagementScripts = "~/bundles/domainmanagement/index-scripts";

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                DomainManagementStyles,
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.css",
                "~/Content/MultipleSelect/fanex.multiSelect-custom.less",
                "~/Areas/DomainManagement/Styles/domainManagement.less");

            bundles.Add(new ScriptBundle(DomainManagementScripts).Include(
                "~/Scripts/Libs/Fanex.Multiselect/fanex.multiSelect.js",
                "~/Areas/DomainManagement/Scripts/domainManagementAddOrEdit.js",
                "~/Areas/DomainManagement/Scripts/domainManagement.js"));
        }
    }
}
