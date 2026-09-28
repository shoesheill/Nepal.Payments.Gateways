namespace Nepal.Payments.Gateways.Models.eSewa
{
    /// <summary>Query for eSewa's transaction-status API — the server-side way to confirm a payment.</summary>
    public class StatusRequest
    {
        public string ProductCode { get; set; }
        public string TotalAmount { get; set; }
        public string TransactionUuid { get; set; }
    }
}
