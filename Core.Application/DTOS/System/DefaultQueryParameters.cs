using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.DTOS.System
{
    public class DefaultQueryParameters
    {
        public DefaultQueryParameters()
        {
            PageNumber = 1;
            PageSize = 700;
        }

        public DefaultQueryParameters(int pageNumber, int pageSize, string orderBy, string search)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
            Search = search;
        }


        [FromQuery(Name = "pageNumber")]
        [DisplayName("pageNumber")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "pageSize")]
        [DisplayName("pageSize")]
        public int PageSize { get; set; }

        [FromQuery(Name = "orderBy")]
        [DisplayName("orderBy")]
        public string OrderBy { get; set; } = string.Empty;

        [FromQuery(Name = "search")]
        [DisplayName("search")]

        public string Search { get; set; } = string.Empty;
    }
}
