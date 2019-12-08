using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public class TakealotPromotionJson
    {
        public int status_code { get; set; }
        public string result { get; set; }
        public List<Response> response { get; set; }
    }

    public class Images
    {
        public string place_holder { get; set; }
        public string top_banner { get; set; }
    }

    public class Ui
    {
        public Images images { get; set; }
    }

    public class Response
    {
        public string display_name { get; set; }
        public string date_end { get; set; }
        public string date_start { get; set; }
        public bool is_active { get; set; }
        public string short_display_name { get; set; }
        public Ui ui { get; set; }
        public int promotion_id { get; set; }
        public int qualifying_quantity { get; set; }
        public int group_id { get; set; }
        public string slug { get; set; }
        public double? promotion_price { get; set; }
    }
}
