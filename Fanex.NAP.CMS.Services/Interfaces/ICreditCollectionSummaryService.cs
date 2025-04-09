namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Fanex.NAP.CMS.Models.Common;
    using Models.CreditCollectionSummary;

    public interface ICreditCollectionSummaryService
    {
        CreditCollectionSummaryReport Generate(CreditCollectionSummaryFilter filter);

        ICommandResult Save(SaveCreditStatusParam param);
    }
}
