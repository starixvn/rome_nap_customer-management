namespace Fanex.Nap.CMS.Customers.Models.ProblemAccount
{
    using Infrastructure.Models;
    using Resources;

    public class MarkNewResult : Result
    {
        public MarkNewResult(int code, string message, string successUsernames, string errorUsernames)
        {
            Code = code;

            if (code == 0)
            {
                Message = ProblemAccountResource.MarkNewSuccessfully;
            }
            else if (code == 500)
            {
                if (!string.IsNullOrWhiteSpace(successUsernames))
                {
                    Message = string.Format(ProblemAccountResource.MarkNewPartialSuccessfully, successUsernames);
                }

                ErrorMessage = string.Format(ProblemAccountResource.MarkNewPartialError, errorUsernames);
            }
            else
            {
                ErrorMessage = message;
            }
        }

        public string ErrorMessage { get; }
    }
}
