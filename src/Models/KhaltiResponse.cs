namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the response from Khalti payment verification.
    /// </summary>
    public class KhaltiResponse
    {
        /// <summary>
        /// Gets or sets the payment index (unique identifier for the payment).
        /// </summary>
        public string Pidx { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the payment.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the transaction ID.
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the transaction fee.
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the payment has been refunded.
        /// </summary>
        public bool Refunded { get; set; }
    }
}
