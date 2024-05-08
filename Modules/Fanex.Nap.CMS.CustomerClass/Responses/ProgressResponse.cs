namespace Fanex.Nap.CMS.CustomerClass.Responses
{
    internal class ProgressResponse
    {
        public ProgressResponse(int countBatch, int initialBatch)
        {
            CountBatch = countBatch;
            InitialBatch = initialBatch;
        }
        
        public int CountBatch { get; set; }
        public int InitialBatch { get; set; }
    }
}
