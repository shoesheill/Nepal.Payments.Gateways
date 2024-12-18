using System;
using System.Collections.Generic;
using System.Text;

namespace payment_gateway_nepal
{
	public class KhaltiInitResponse
	{
        public string pidx { get; set; }
        public string payment_url { get; set; }
        public DateTime expires_at { get; set; }
        public int expires_in { get; set; }
    }
}
