namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the amount breakdown for Khalti payment.
    /// </summary>
    public class KhaltiAmountBreakdown
    {
        /// <summary>
        /// Gets or sets the label for the amount breakdown.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the amount in paisa (smallest currency unit).
        /// </summary>
        public int Amount { get; set; }
    }
}
