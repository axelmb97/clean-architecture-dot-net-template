

using System.Text.Json.Serialization;

namespace Application.Common.Dtos.Filters
{
    public class PaginationHeadersDto
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("firstPage")]
        public int FirstPage { get; set; }
        [JsonPropertyName("lastPage")]
        public int LastPage { get; set; }
        [JsonPropertyName("page")]
        public int Page { get; set; }
        [JsonPropertyName("nextPage")]
        public int NextPage { get; set; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }
        [JsonPropertyName("previousPage")]
        public int PreviousPage { get; set; }

        public PaginationHeadersDto(SortedFiltersDto filter, int totalToResponse)
        {
            Total = totalToResponse;
            PageSize = filter.PageSize != 0 ? filter.PageSize : 10;
            TotalPages = (int)Math.Round((decimal)totalToResponse / PageSize, MidpointRounding.ToPositiveInfinity);
            FirstPage = 1;
            LastPage = TotalPages;
            Page = filter.Page;
            NextPage = LastPage <= Page ? LastPage : Page + 1;
            PreviousPage = FirstPage >= Page ? FirstPage : Page - 1;
        }

        public override bool Equals(object obj)
        {
            return obj is PaginationHeadersDto model &&
                   Total == model.Total &&
                   TotalPages == model.TotalPages &&
                   FirstPage == model.FirstPage &&
                   LastPage == model.LastPage &&
                   Page == model.Page &&
                   NextPage == model.NextPage &&
                   PageSize == model.PageSize &&
                   PreviousPage == model.PreviousPage;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Total, TotalPages, FirstPage, LastPage, Page, NextPage, PageSize, PreviousPage);
        }
    }
}
