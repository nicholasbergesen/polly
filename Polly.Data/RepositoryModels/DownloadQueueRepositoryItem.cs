namespace Polly.Data
{
    public class DownloadQueueRepositoryItem
    {
        public int websiteId;
        public string DownloadLink;

        public override string ToString()
        {
            return $"{websiteId},{DownloadLink}";
        }
    }
}
