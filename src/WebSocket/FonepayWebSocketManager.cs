using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Manager;
using Nepal.Payments.Gateways.Enum;

namespace Nepal.Payments.Gateways.WebSocket
{
    public class FonepayWebSocketManager : IPaymentWebSocketManager
    {
        public event EventHandler<PaymentStatusEventArgs> StatusChanged;
        public event EventHandler<PaymentVerifiedEventArgs> PaymentVerified;
        public event EventHandler<PaymentTimeoutEventArgs> PaymentTimeout;
        public event EventHandler<PaymentErrorEventArgs> PaymentError;
        public event EventHandler<PaymentCancelledEventArgs> PaymentCancelled;

        private readonly Dictionary<string, ClientWebSocket> _activeConnections = new Dictionary<string, ClientWebSocket>();
        private readonly Dictionary<string, CancellationTokenSource> _cancellationTokens = new Dictionary<string, CancellationTokenSource>();
        private readonly Dictionary<string, PaymentCredentials> _paymentCredentials = new Dictionary<string, PaymentCredentials>();
        private readonly object _lockObject = new object();

        public async Task StartMonitoringAsync(string prn, string webSocketUrl, PaymentCredentials credentials, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(prn) || string.IsNullOrEmpty(webSocketUrl) || credentials == null)
                return;

            // Stop existing monitoring for this PRN
            await StopMonitoringAsync(prn);

            lock (_lockObject)
            {
                var cts = new CancellationTokenSource();
                _cancellationTokens[prn] = cts;
                _paymentCredentials[prn] = credentials;
            }

            // Start monitoring in background
            _ = Task.Run(async () => await MonitorPaymentAsync(prn, webSocketUrl, _cancellationTokens[prn].Token), cancellationToken);
        }

        public async Task StopMonitoringAsync(string prn)
        {
            CancellationTokenSource? cts = null;
            ClientWebSocket webSocket = null;

            lock (_lockObject)
            {
                if (_cancellationTokens.TryGetValue(prn, out cts))
                {
                    cts.Cancel();
                    _cancellationTokens.Remove(prn);
                }

                if (_activeConnections.TryGetValue(prn, out webSocket))
                {
                    _activeConnections.Remove(prn);
                }

                _paymentCredentials.Remove(prn);
            }

            if (webSocket != null)
            {
                try
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Monitoring stopped", CancellationToken.None);
                    }
                }
                catch
                {
                    // Ignore close errors
                }
                finally
                {
                    webSocket.Dispose();
                }
            }

            cts?.Dispose();
        }

        public bool IsMonitoring(string prn)
        {
            lock (_lockObject)
            {
                return _activeConnections.ContainsKey(prn) && 
                       _activeConnections[prn].State == WebSocketState.Open;
            }
        }

        private async Task MonitorPaymentAsync(string prn, string webSocketUrl, CancellationToken cancellationToken)
        {
            ClientWebSocket webSocket = null;
            try
            {
                webSocket = new ClientWebSocket();

                lock (_lockObject)
                {
                    _activeConnections[prn] = webSocket;
                }

                // Set timeout (15 minutes)
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(15));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken, timeoutCts.Token);

                // Connect to WebSocket
                await webSocket.ConnectAsync(new Uri(webSocketUrl), combinedCts.Token);

                // Notify connection established
                StatusChanged?.Invoke(this, new PaymentStatusEventArgs
                {
                    Prn = prn,
                    PaymentStatus = "websocket_connected",
                    RawMessage = "WebSocket connected successfully"
                });

                // Listen for messages
                var buffer = new byte[4096];
                while (webSocket.State == WebSocketState.Open && !combinedCts.Token.IsCancellationRequested)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), combinedCts.Token);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await HandleWebSocketMessage(prn, message);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // Monitoring was cancelled
            }
            catch (OperationCanceledException)
            {
                // Timeout occurred
                PaymentTimeout?.Invoke(this, new PaymentTimeoutEventArgs 
                { 
                    Prn = prn,
                    TimeoutDuration = TimeSpan.FromMinutes(15)
                });
            }
            catch (Exception ex)
            {
                PaymentError?.Invoke(this, new PaymentErrorEventArgs 
                { 
                    Prn = prn,
                    ErrorMessage = ex.Message,
                    Exception = ex
                });
            }
            finally
            {
                if (webSocket != null)
                {
                    lock (_lockObject)
                    {
                        _activeConnections.Remove(prn);
                        _cancellationTokens.Remove(prn);
                    }
                    
                    webSocket.Dispose();
                }
            }
        }

        private async Task HandleWebSocketMessage(string prn, string message)
        {
            try
            {
                var eventArgs = new PaymentStatusEventArgs
                {
                    Prn = prn,
                    RawMessage = message
                };

                // Parse the main message
                using (var document = JsonDocument.Parse(message))
                {
                    var root = document.RootElement;

                    // Parse transaction status
                    if (root.TryGetProperty("transactionStatus", out var transactionStatusElement))
                    {
                        // Handle nested JSON string - get the string value and parse it separately
                        string transactionStatusJson = null;

                        if (transactionStatusElement.ValueKind == JsonValueKind.String)
                        {
                            transactionStatusJson = transactionStatusElement.GetString();
                        }
                        else if (transactionStatusElement.ValueKind == JsonValueKind.Object)
                        {
                            transactionStatusJson = transactionStatusElement.GetRawText();
                        }

                        // Parse the transaction status in a separate using block
                        if (!string.IsNullOrEmpty(transactionStatusJson))
                        {
                            using (var statusDocument = JsonDocument.Parse(transactionStatusJson))
                            {
                                var transactionStatus = statusDocument.RootElement;

                                // Check for QR verification
                                if (transactionStatus.TryGetProperty("qrVerified", out var qrVerifiedElement) &&
                                    qrVerifiedElement.GetBoolean())
                                {
                                    eventArgs.QrVerified = true;
                                    eventArgs.PaymentStatus = "qr_verified";
                                }

                                // Check for payment success/failure
                                if (transactionStatus.TryGetProperty("paymentSuccess", out var paymentSuccessElement))
                                {
                                    var paymentSuccess = paymentSuccessElement.GetBoolean();
                                    eventArgs.PaymentSuccess = paymentSuccess;
                                    eventArgs.PaymentStatus = paymentSuccess ? "payment_success" : "payment_failed";
                                }

                                // Check for cancellation
                                if (transactionStatus.TryGetProperty("cancelled", out var cancelledElement) && cancelledElement.GetBoolean())
                                {
                                    eventArgs.PaymentStatus = "payment_cancelled";

                                    // Fire PaymentCancelled event
                                    var cancelReason = transactionStatus.TryGetProperty("message", out var msgElement)
                                        ? msgElement.GetString() ?? "Payment cancelled"
                                        : "Payment cancelled";

                                    PaymentCancelled?.Invoke(this, new PaymentCancelledEventArgs
                                    {
                                        Prn = prn,
                                        Reason = cancelReason,
                                        CancelledBy = "user"
                                    });
                                }

                                // Add additional data
                                eventArgs.AdditionalData = System.Text.Json.JsonSerializer.Deserialize<object>(transactionStatus.GetRawText());
                            }
                        }
                    }
                }

                // Fire the event (outside the using blocks)
                StatusChanged?.Invoke(this, eventArgs);

                // Auto-verify payment if successful
                if (eventArgs.PaymentSuccess == true)
                {
                    _ = Task.Run(async () => await VerifyPaymentAsync(prn));
                }
            }
            catch (Exception ex)
            {
                PaymentError?.Invoke(this, new PaymentErrorEventArgs
                {
                    Prn = prn,
                    ErrorMessage = $"Error parsing WebSocket message: {ex.Message}",
                    Exception = ex
                });
            }
        }

        private async Task VerifyPaymentAsync(string prn)
        {
            try
            {
                PaymentCredentials credentials = null;
                lock (_lockObject)
                {
                    _paymentCredentials.TryGetValue(prn, out credentials);
                }

                if (credentials == null)
                {
                    PaymentError?.Invoke(this, new PaymentErrorEventArgs
                    {
                        Prn = prn,
                        ErrorMessage = "Payment credentials not found for verification"
                    });
                    return;
                }

                // Create payment manager for verification
                var paymentManager = new PaymentManager(
                    paymentMethod: PaymentMethod.FonePay,
                    paymentVersion: PaymentVersion.V2,
                    paymentMode: credentials.SandboxMode ? PaymentMode.Sandbox : PaymentMode.Production,
                    secretKey: credentials.SecretKey
                );

                // Prepare verification data
                var verificationData = new Dictionary<string, string>
                {
                    ["prn"] = prn,
                    ["merchantCode"] = credentials.MerchantCode,
                    ["username"] = credentials.Username,
                    ["password"] = credentials.Password
                };

                // Verify payment
                var response = await paymentManager.VerifyPaymentAsync<dynamic>(
                    System.Text.Json.JsonSerializer.Serialize(verificationData)
                );

                // Fire PaymentVerified event
                PaymentVerified?.Invoke(this, new PaymentVerifiedEventArgs
                {
                    Prn = prn,
                    Success = response.Success,
                    VerificationData = response.Data,
                    ErrorMessage = response.Success ? null : response.Message
                });
            }
            catch (Exception ex)
            {
                PaymentError?.Invoke(this, new PaymentErrorEventArgs
                {
                    Prn = prn,
                    ErrorMessage = $"Error verifying payment: {ex.Message}",
                    Exception = ex
                });
            }
        }

        public void Dispose()
        {
            var prnsToStop = new List<string>();
            
            lock (_lockObject)
            {
                prnsToStop.AddRange(_activeConnections.Keys);
            }

            foreach (var prn in prnsToStop)
            {
                _ = StopMonitoringAsync(prn);
            }

            lock (_lockObject)
            {
                foreach (var cts in _cancellationTokens.Values)
                {
                    cts.Cancel();
                    cts.Dispose();
                }
                
                _activeConnections.Clear();
                _cancellationTokens.Clear();
                _paymentCredentials.Clear();
            }
        }
    }
}
