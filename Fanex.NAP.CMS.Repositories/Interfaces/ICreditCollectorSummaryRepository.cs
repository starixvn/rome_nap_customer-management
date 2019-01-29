namespace Fanex.NAP.CMS.Repositories.Interfaces
{
    using Criteria.CreditCollectionSummary;
    using Models.CreditCollectionSummary;

    public interface ICreditCollectionSummaryRepository
    {
        CreditCollectionSummaryReport Generate(CreditCollectionSummaryCriteria criteria);
    }
}
