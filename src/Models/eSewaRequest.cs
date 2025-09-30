namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the request model for eSewa payment processing.
    /// </summary>
    public class EsewaRequest
    {
        /// <summary>
        /// Gets or sets the payment amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Gets or sets the total amount (amount + tax + charges).
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the unique transaction identifier.
        /// </summary>
        public string TransactionUuid { get; set; }

        /// <summary>
        /// Gets or sets the product code.
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the product service charge.
        /// </summary>
        public decimal ProductServiceCharge { get; set; }

        /// <summary>
        /// Gets or sets the product delivery charge.
        /// </summary>
        public decimal ProductDeliveryCharge { get; set; }

        /// <summary>
        /// Gets or sets the success callback URL.
        /// </summary>
        public string SuccessUrl { get; set; }

        /// <summary>
        /// Gets or sets the failure callback URL.
        /// </summary>
        public string FailureUrl { get; set; }

        /// <summary>
        /// Gets or sets the signed field names for signature generation.
        /// </summary>
        public string SignedFieldNames { get; set; }

        /// <summary>
        /// Gets or sets the generated signature.
        /// </summary>
        public string Signature { get; set; }
    }
}
