namespace DemoReactAPI.Dtos
{
    public class PagedResponse<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PagedResponse(PaginatedList<T> paginatedList)
        {
            Items = paginatedList;
            PageIndex = paginatedList.PageIndex;
            TotalPages = paginatedList.TotalPages;
            PageSize = paginatedList.PageSize;
            TotalCount = paginatedList.TotalCount;
        }
    }

}
