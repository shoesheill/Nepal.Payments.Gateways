namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the response model from eSewa payment gateway.
    /// </summary>
    public class EsewaResponse
    {
        /// <summary>
        /// Gets or sets the payment status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the signature for verification.
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the transaction code.
        /// </summary>
        public string TransactionCode { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the transaction UUID.
        /// </summary>
        public string TransactionUuid { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the signed field names.
        /// </summary>
        public string SignedFieldNames { get; set; }
    }
}
