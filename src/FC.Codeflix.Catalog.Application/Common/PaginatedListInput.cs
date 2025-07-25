﻿using FC.Codeflix.Catalog.Domain.SeedWork.SearchableRepository;

namespace FC.Codeflix.Catalog.Application.Common
{
    public abstract class PaginatedListInput
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public SearchOrder SortBy { get; set; }

        public PaginatedListInput(int page, int perPage, string search, string sort, SearchOrder sortBy)
        {
            Page = page;
            PerPage = perPage;
            Search = search;
            Sort = sort;
            SortBy = sortBy;
        }
    }
}
