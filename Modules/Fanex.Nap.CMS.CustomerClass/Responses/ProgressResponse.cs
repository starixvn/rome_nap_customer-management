namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    internal class ProgressResponse : Result
    {
        public ProgressResponse(int countBatch, int initialBatch, int code, string message) : base(code, message)
        {
            CountBatch = countBatch;
            InitialBatch = initialBatch;
        }
        
        public int CountBatch { get; set; }
        public int InitialBatch { get; set; }
    }
}
