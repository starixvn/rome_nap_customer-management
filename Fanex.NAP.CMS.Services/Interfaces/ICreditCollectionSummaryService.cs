namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Framework.Command;
    using Models.CreditCollectionSummary;

    public interface ICreditCollectionSummaryService
    {
        CreditCollectionSummaryReport Generate(CreditCollectionSummaryFilter filter);

        ICommandResult Save(SaveCreditStatusParam param);
    }
}
