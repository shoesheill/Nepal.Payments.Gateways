namespace payment_gateway_nepal
{
    public class eSewaResponse
    {
        public string status { get; set; }
        public string signature { get; set; }
        public  string transaction_code { get; set; }
        public decimal total_amount { get; set; }
        public  string transaction_uuid { get; set; }
        public  string product_code { get; set; }
        public  string signed_field_names { get; set; }
    }
}
