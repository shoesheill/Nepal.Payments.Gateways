namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents customer information for Khalti payment.
    /// </summary>
    public class KhaltiCustomerInfo
    {
        /// <summary>
        /// Gets or sets the customer's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the customer's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        public string Phone { get; set; }
    }
}
