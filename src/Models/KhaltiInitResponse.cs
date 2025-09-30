using System;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Represents the response from Khalti payment initiation.
    /// </summary>
    public class KhaltiInitResponse
    {
        /// <summary>
        /// Gets or sets the payment index (unique identifier for the payment).
        /// </summary>
        public string Pidx { get; set; }

        /// <summary>
        /// Gets or sets the payment URL where the user should be redirected.
        /// </summary>
        public string PaymentUrl { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the payment.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Gets or sets the expiration time in seconds.
        /// </summary>
        public int ExpiresIn { get; set; }
    }
}
