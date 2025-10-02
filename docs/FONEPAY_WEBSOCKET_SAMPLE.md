## Fonepay WebSocket - App Integration Sample

This sample shows how your application (not the NuGet package) should connect to Fonepay's WebSocket URL returned from the QR initiation response, and parse messages using models provided by this package.

### Prerequisites

- Generate a QR via `PaymentManager.InitiatePaymentAsync` (Fonepay) and read `thirdpartyQrWebSocketUrl` from the response
- Add `System.Net.WebSockets` (for ASP.NET Core) or a client WebSocket library suitable for your app type

### Minimal Sample

```csharp
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Models.Fonepay;
using Newtonsoft.Json;

// 1) Generate QR via PaymentManager
var pm = new PaymentManager(PaymentMethod.FonePay, PaymentVersion.V1, PaymentMode.Sandbox, Fonepay_SecretKey);
var qrRequest = new QrRequest
{
    Amount = "14.00",
    Remarks1 = "Test Payment",
    Remarks2 = "QR Integration",
    Prn = Guid.NewGuid().ToString(),
    MerchantCode = Fonepay_MerchantCode,
    Username = Fonepay_Username,
    Password = Fonepay_Password
};

var qrInit = await pm.InitiatePaymentAsync<PaymentResult>(qrRequest);
var qrData = JsonConvert.DeserializeObject<QrResponse>(JsonConvert.SerializeObject(qrInit.Data));
var wsUrl = qrData.ThirdpartyQrWebSocketUrl; // Provided by Fonepay response

// 2) Connect to WebSocket from your app
using var socket = new ClientWebSocket();
await socket.ConnectAsync(new Uri(wsUrl), CancellationToken.None);

// 3) Simple receive loop and parse using package models
var buffer = new byte[8192];
while (socket.State == WebSocketState.Open)
{
    var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
    if (result.MessageType == WebSocketMessageType.Text)
    {
        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var wsMsg = JsonConvert.DeserializeObject<WebSocketMessage>(json);
        var status = JsonConvert.DeserializeObject<TransactionStatus>(wsMsg.TransactionStatus);

        if (status.QrVerified == true)
        {
            // The QR has been scanned and verified on the Fonepay side
        }

        if (status.PaymentSuccess == true)
        {
            // Success: update order, notify user, etc.
            break;
        }

        if (status.PaymentSuccess == false)
        {
            // Failed: show error and exit loop
            break;
        }
    }
}
```

### Notes

- Keep sockets owned by your app; this package remains stateless and provides models/signatures
- Always confirm final payment status via the `thirdPartyDynamicQrGetStatus` API if needed


