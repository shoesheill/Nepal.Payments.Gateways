namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents product details for Khalti payment.
    /// </summary>
    public class KhaltiProductDetail
    {
        /// <summary>
        /// Gets or sets the product identity/ID.
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total price in paisa (smallest currency unit).
        /// </summary>
        public int TotalPrice { get; set; }

        /// <summary>
        /// Gets or sets the product quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price in paisa (smallest currency unit).
        /// </summary>
        public int UnitPrice { get; set; }
    }
}
