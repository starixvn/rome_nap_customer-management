namespace Fanex.NAP.CMS.Models.OpenCloseMember
{
    public class UpdateResult
    {
        public UpdateResult(
            int code, 
            string message, 
            string listCustRemove,
            int updatedCount)
        {
            Code = code;
            Message = message;
            ListCustRemove = listCustRemove;
            UpdatedCount = updatedCount;
        }

        public int Code { get; }

        public string Message { get; }

        public string ListCustRemove { get; }

        public int UpdatedCount { get; }
    }
}