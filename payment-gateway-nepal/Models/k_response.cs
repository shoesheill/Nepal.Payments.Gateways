using System;
using System.Collections.Generic;
using System.Text;

namespace payment_gateway_nepal.Models
{
	public class k_response
	{
		public string pidx { get; set; }

		public decimal total_amount { get; set; }

		public string status { get; set; }

		public string transaction_id { get; set; }
		public decimal fee { get; set; }
		public bool refunded { get; set; }
	}
}
