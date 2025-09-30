namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the environment mode for payment processing.
    /// </summary>
    public enum PaymentMode
    {
        /// <summary>
        /// Sandbox/test environment for development and testing.
        /// </summary>
        Sandbox,

        /// <summary>
        /// Production environment for live transactions.
        /// </summary>
        Production
    }
}
