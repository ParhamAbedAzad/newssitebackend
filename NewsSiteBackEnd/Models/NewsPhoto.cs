using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class NewsPhoto
    {
        public int Id { get; set; }
        public int? NewsId { get; set; }
        public string PhotoUrl { get; set; }

        public News News { get; set; }
    }
}
