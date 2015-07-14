namespace SimpleFund.Web.Models
{
    public class PagedFilter
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string SortBy { get; set; }
        public bool Desc { get; set; }
    }
}