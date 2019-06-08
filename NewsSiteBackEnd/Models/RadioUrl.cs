using System;
using System.Collections.Generic;

namespace NewsSiteBackEnd.Models
{
    public partial class RadioUrl
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime? Date { get; set; }
    }
}
