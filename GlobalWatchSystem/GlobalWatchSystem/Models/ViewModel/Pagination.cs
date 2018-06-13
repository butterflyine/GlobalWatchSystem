namespace GlobalWatchSystem.Models.ViewModel
{
    public class Pagination
    {
        public const int DefaultPageSize = 20;

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }

        public int PageSize
        {
            get { return DefaultPageSize; }
        }

        public int TotalPageCount
        {
            get { return TotalCount%PageSize == 0 ? TotalCount/PageSize : 1 + TotalCount/PageSize; }
        }

        public bool First
        {
            get { return CurrentPage == 1; }
        }

        public bool Last
        {
            get { return CurrentPage == TotalPageCount; }
        }
    }
}