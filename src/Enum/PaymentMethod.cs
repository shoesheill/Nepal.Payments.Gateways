namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the different payment gateway providers available in Nepal.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// eSewa payment gateway.
        /// </summary>
        Esewa,

        /// <summary>
        /// Khalti payment gateway.
        /// </summary>
        Khalti,

        /// <summary>
        /// IMEPay payment gateway (planned for future implementation).
        /// </summary>
        IMEPay,

        /// <summary>
        /// FonePay payment gateway (planned for future implementation).
        /// </summary>
        FonePay
    }
}
