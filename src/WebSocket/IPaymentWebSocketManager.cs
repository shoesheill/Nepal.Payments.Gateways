using System;
using System.Threading;
using System.Threading.Tasks;

namespace Nepal.Payments.Gateways.WebSocket
{
    public interface IPaymentWebSocketManager : IDisposable
    {
        event EventHandler<PaymentStatusEventArgs> StatusChanged;
        event EventHandler<PaymentVerifiedEventArgs> PaymentVerified;
        event EventHandler<PaymentTimeoutEventArgs> PaymentTimeout;
        event EventHandler<PaymentErrorEventArgs> PaymentError;
        event EventHandler<PaymentCancelledEventArgs> PaymentCancelled;

        Task StartMonitoringAsync(string prn, string webSocketUrl, PaymentCredentials credentials, CancellationToken cancellationToken = default);
        Task StopMonitoringAsync(string prn);
        bool IsMonitoring(string prn);
    }

    public class PaymentStatusEventArgs : EventArgs
    {
        public string Prn { get; set; } = string.Empty;
        public bool? QrVerified { get; set; }
        public bool? PaymentSuccess { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string RawMessage { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public object AdditionalData { get; set; }
    }

    public class PaymentTimeoutEventArgs : EventArgs
    {
        public string Prn { get; set; } = string.Empty;
        public TimeSpan TimeoutDuration { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class PaymentErrorEventArgs : EventArgs
    {
        public string Prn { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public Exception Exception { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class PaymentVerifiedEventArgs : EventArgs
    {
        public string Prn { get; set; } = string.Empty;
        public bool Success { get; set; }
        public object VerificationData { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class PaymentCancelledEventArgs : EventArgs
    {
        public string Prn { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string CancelledBy { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class PaymentCredentials
    {
        public string SecretKey { get; set; } = string.Empty;
        public string MerchantCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool SandboxMode { get; set; }
    }
}
