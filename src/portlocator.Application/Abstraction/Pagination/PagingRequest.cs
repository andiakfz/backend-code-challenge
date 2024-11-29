namespace portlocator.Application.Abstraction.Pagination
{
    public class PagingRequest
    {
        public string? Search { get; set; }
        public string? Filter { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
        public string? OrderBy { get; set; }
        public OrderDirection OrderDirection { get; set; } = OrderDirection.Asc;
    }

    public enum OrderDirection
    {
        Asc,
        Desc
    }
}
