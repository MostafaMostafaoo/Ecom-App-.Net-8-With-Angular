﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.core.Sharing
{
    public class ProductParams
    {
        //string? sort, int? categoryId, int pageSize, int PageNumber
        public string? Sort { get; set; }

        public int? CategoryId { get; set; }

        public string? Search { get; set; } = string.Empty;

        public int MaxPageSize { get; set; } = 6;

        private int _pageSize = 3;

        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public int PageNumber { get; set; } = 1;
    }
}
