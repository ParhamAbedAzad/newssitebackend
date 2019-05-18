using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class News
    {
        public News()
        {
            Comments = new HashSet<Comments>();
            NewsPhoto = new HashSet<NewsPhoto>();
            Tags = new HashSet<Tags>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? AdminId { get; set; }

        public Admins Admin { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<NewsPhoto> NewsPhoto { get; set; }
        public ICollection<Tags> Tags { get; set; }
    }
}
