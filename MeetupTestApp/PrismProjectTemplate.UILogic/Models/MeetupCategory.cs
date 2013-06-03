using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupTestClient.UILogic.Models
{
    public class MeetupCategory
    {
        public ObservableCollection<Category> results { get; set; }
        public CatMeta meta { get; set; }
    }

    public class CatMeta
    {
        public string lon { get; set; }
        public int count { get; set; }
        public string signed_url { get; set; }
        public string link { get; set; }
        public string next { get; set; }
        public int total_count { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public long updated { get; set; }
        public string description { get; set; }
        public string method { get; set; }
        public string lat { get; set; }
    }
}
