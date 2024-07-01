namespace HotelListingAPI.Models
{
    public class QueryParameters
    {
        public string Filter { get; set; } = "";

        public string OrderBy { get; set; } = "";
        public string SortOrder { get; set; } = "";

        public string SearchTerm { get; set; } = "";

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}
