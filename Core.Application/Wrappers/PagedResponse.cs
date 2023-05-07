namespace Core.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalItems { get; set; }



        public PagedResponse(T data, int pageNumber, int pageSize, int totalPage, int totalItems)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            TotalPage = totalPage;
            Message = null;
            Succeeded = true;
            Errors = null;
            TotalItems = totalItems;
        }
    }
}
