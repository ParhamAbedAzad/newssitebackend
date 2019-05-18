using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime? Date { get; set; }
        public int? UserId { get; set; }
        public int? NewsId { get; set; }
        public News News { get; set; }
        public Users User { get; set; }
    }
}
