namespace payment_gateway_nepal
{
	public class KhaltiResponse
	{
		public string pidx { get; set; }
		public decimal total_amount { get; set; }
		public string status { get; set; }
		public string transaction_id { get; set; }
		public decimal fee { get; set; }
		public bool refunded { get; set; }
	}
}
