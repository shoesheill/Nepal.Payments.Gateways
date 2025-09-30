namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the different actions that can be performed on a payment.
    /// </summary>
    public enum PaymentAction
    {
        /// <summary>
        /// Initiate a new payment transaction.
        /// </summary>
        ProcessPayment,

        /// <summary>
        /// Verify the status of an existing payment transaction.
        /// </summary>
        VerifyPayment,

        /// <summary>
        /// Check the current status of a payment transaction.
        /// </summary>
        CheckPayment
    }
}
