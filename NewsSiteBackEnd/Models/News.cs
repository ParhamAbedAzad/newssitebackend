using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? AuthorId { get; set; }
    }
}
