namespace Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting
{
    public class GetCompanyBetLimitSettingViewModels
    {
        public bool IsCredit { get; set; }
        public string ProductIds { get; set; }
        public string Currencies { get; set; }
        
        public int PageIndex { get; set; }
        
        public int PageSize { get; set; }
    }
}