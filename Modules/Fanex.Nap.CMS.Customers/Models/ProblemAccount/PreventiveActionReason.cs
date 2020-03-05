namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    public class PreventiveActionReason
    {
        public PreventiveActionReason(
            int preventiveActionId,
            string preventiveActionName,
            int reasonId,
            string reasonName)
        {
            PreventiveActionId = preventiveActionId;
            PreventiveActionName = preventiveActionName;
            ReasonId = reasonId;
            ReasonName = reasonName;
        }

        public int PreventiveActionId { get; }

        public string PreventiveActionName { get; }

        public int ReasonId { get; }

        public string ReasonName { get; }
    }
}
