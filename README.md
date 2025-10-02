# Nepal.Payments.Gateways

Easy-peasy payments gateways package for Nepal 🇳🇵 — integrate eSewa, Khalti, and Fonepay (Dynamic QR) without jhanjhat. Just plug, pay, and chill! 💸

> Nepali ma bhannu parda "Paisa Tirne System" ho! 💰

> **Note:** yo implement garna jinsi saman haru kei chaidaina

[![NuGet](https://img.shields.io/nuget/v/Nepal.Payments.Gateways.svg)](https://www.nuget.org/packages/Nepal.Payments.Gateways/)
[![Downloads](https://img.shields.io/nuget/dt/Nepal.Payments.Gateways.svg)](https://www.nuget.org/packages/Nepal.Payments.Gateways/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## 🚀 Features

- ✅ **Multiple Payment Gateways** - Khalti, eSewa, Fonepay support 
- ✅ **Real-time Monitoring** - WebSocket-based payment tracking for Fonepay 
- ✅ **Auto-verification** - Automatic payment verification when successful
- ✅ **Sandbox & Production** - Full support for both environments 

>Yesto features cha ki "Wow!" bhanna man lagcha!
## 📦 Installation

```bash
dotnet add package Nepal.Payments.Gateways
```

<!-- Package install garna jati sajilo cha, momo khana jati sajilo! Just one command! 🥟 -->

## ⚡ Quick Start

### 1. Configure Services

```csharp
// Program.cs
using Nepal.Payments.Gateways.Extensions;

builder.Services.AddNepalPaymentGateways();
```

### 2. Add Configuration

```json
{
  "PaymentGateways": {
    "SandboxMode": true,
    "Khalti": { "SecretKey": "your-khalti-secret-key" },
    "eSewa": { "SecretKey": "your-esewa-secret-key" },
    "Fonepay": {
      "SecretKey": "your-fonepay-secret-key",
      "MerchantCode": "your-merchant-code",
      "Username": "your-username",
      "Password": "your-password"
    }
  }
}
```

### 3. Basic Payment (Khalti/eSewa)

```csharp
var paymentManager = new PaymentManager(
    PaymentMethod.Khalti,
    PaymentVersion.V2,
    PaymentMode.Sandbox,
    "your-secret-key"
);

var response = await paymentManager.InitiatePaymentAsync<dynamic>(paymentRequest);

if (response.Success)
{
    // Redirect to response.Data.PaymentUrl
}
```

### 4. Real-time QR Payment (Fonepay)

```csharp
// Generate QR
var qrResponse = await paymentManager.InitiatePaymentAsync<QrResponse>(qrRequest);

// Start real-time monitoring
webSocketManager.StatusChanged += (sender, args) => {
    // Handle real-time payment updates
};

await webSocketManager.StartMonitoringAsync(prn, webSocketUrl, credentials);
```

## 🏗️ Architecture

<!-- Architecture design jasto ramro cha, jasto ki Kathmandu ko traffic system... wait, that's a bad example! 😅 -->
The package uses an **event-driven architecture** that separates payment processing from UI notifications:

```
Payment Gateway → NuGet Package → Your Events → Your UI Choice
                                      ↓
                              (SignalR, WebSockets, Polling, etc.)
```

This design gives you **maximum flexibility** in how you handle real-time notifications.

> **Euta kura bhanam?** This architecture is more organized than??? hamro desko system bhanda
> <!-- Actually follows proper sequence, unlike bus ma chadhne queue! -->

## 💳 Supported Gateways

<!-- Tini jana popular gateways - jasto ki Nepal ma tini jana popular dal! -->
| Gateway | Versions | Payment Types | Real-time Monitoring |
|---------|----------|---------------|---------------------|
| **Khalti** | V1, V2 | Redirect-based | ❌ <!-- Traditional jasto ki khukuri! --> |
| **eSewa** | V1, V2 | Redirect-based | ❌ <!-- Reliable jasto ki dhido! --> |
| **Fonepay** |  | QR-based | ✅ WebSocket + Auto-verification <!-- Modern jasto ki TikTok! --> |

> **Pro Tip:** Fonepay le real-time updates dincha - jasto ki cricket match ko live score, ajkal jasle ni herxan ni ta bujhxan holani! 🏏

## 🔄 Real-time Integration Options

Choose the real-time solution that works best for your application:
> **Bujhena hola hai??** J man lagcha tei gara - Mahabir Pun

### SignalR (Recommended)
> .net ho bhane
- **Best for:** Modern web applications
- **Pros:** Automatic reconnection, broad browser support, easy scaling

### Direct WebSockets
- **Best for:** High-performance applications  
- **Pros:** Lower overhead, full control over messages

### Server-Sent Events (SSE)
- **Best for:** Simple real-time updates
- **Pros:** Simple implementation, automatic reconnection

### Polling
- **Best for:** Simple applications, legacy browser support
- **Pros:** Works everywhere, easy to debug

### Hybrid
- **Best for:** Enterprise applications
- **Pros:** Support multiple client types simultaneously

## 🎯 Events (Fonepay Real-time)

```csharp
webSocketManager.StatusChanged += (sender, args) => {
    // QR scanned, payment status updates
    if (args.QrVerified) Console.WriteLine("QR Code scanned!");
    if (args.PaymentSuccess == true) Console.WriteLine("Payment successful!");
};

webSocketManager.PaymentVerified += (sender, args) => {
    // Auto-verification completed
    Console.WriteLine($"Verification: {args.Success}");
};

webSocketManager.PaymentTimeout += (sender, args) => {
    // 15-minute timeout reached
    Console.WriteLine("Payment timed out");
};

webSocketManager.PaymentError += (sender, args) => {
    // WebSocket or verification errors
    Console.WriteLine($"Error: {args.ErrorMessage}");
};
```

## 🧪 Test Credentials
> **Note:** Yo test credentials vaneko test ko lagi matra use garne ho, natra aaye sauko gaye bauko huna sakcha 😄

### eSewa Sandbox
- **Username:** 9806800001/2/3/4/5
- **Password:** *********👁
- **Token:** ******👁
>**Pro Tip:** yesle kaam garena vane official documentation update vayo ki hernu, yo number maa phone lagdaina call garne prayas nagarnuhola. **Password: Nepal@123** **Token: 123456** 

### Khalti Sandbox  
- **Mobile:** 9800000001/2/3/4/5 
- **Pin:** 1111
- **OTP:** 987654

> **Warning:** Yo test credentials matra ho! Real paisa ma use nagarnuhos - natra ghar ma pitai huncha! 😅
### Fonepay Sandbox
- Contact Fonepay for credentials and merchant setup 
> **Janahitmaa jaari:** yo document update hune bela samma Fonepay Dynamic QR maa kunai pani test credentials chaina 


## 🏗️ Real-time Integration Options

The package is designed to work with **any real-time solution**. Choose what works best for your application:

### Option 1: SignalR (Recommended)
- **Best for:** Modern web applications
- **Pros:** Automatic reconnection, broad browser support, easy scaling
- **Implementation:** Subscribe to package events, send SignalR messages to clients

### Option 2: Direct WebSockets
- **Best for:** High-performance applications
- **Pros:** Lower overhead, full control over messages
- **Implementation:** Subscribe to package events, send WebSocket messages

### Option 3: Server-Sent Events (SSE)
- **Best for:** Simple real-time updates
- **Pros:** Simple implementation, automatic reconnection
- **Implementation:** Subscribe to package events, send SSE messages

### Option 4: Polling
- **Best for:** Simple applications, legacy browser support
- **Pros:** Works everywhere, easy to debug
- **Implementation:** Subscribe to package events, update database/cache

### Option 5: Hybrid
- **Best for:** Enterprise applications
- **Pros:** Support multiple client types simultaneously
- **Implementation:** Handle events and notify via multiple channels

## 🚀 Getting Started

### Step 1: Choose Your Payment Method
- **Khalti/eSewa**: Traditional redirect-based payments
- **Fonepay**: Modern QR-based payments with real-time monitoring

### Step 2: Configure Your Application
- Add package to your project: `dotnet add package Nepal.Payments.Gateways`
- Configure gateway credentials in `appsettings.json`
- Register services: `builder.Services.AddNepalPaymentGateways();`

### Step 3: Implement Payment Logic
- Create `PaymentManager` instances for your chosen gateway
- Handle payment initiation and verification
- For Fonepay: Subscribe to real-time events

### Step 4: Choose Real-time Integration (Fonepay only)
- **SignalR**: For modern web apps (see demo project)
- **WebSockets**: For high-performance scenarios
- **Polling**: For simple implementations
- **SSE**: For mobile-friendly solutions

### Step 5: Handle Events
- Subscribe to `StatusChanged`, `PaymentVerified`, `PaymentTimeout`, `PaymentError`
- Implement your preferred notification method
- Update UI based on payment status changes

## 📚 Documentation
- **[Fonepay Implementation](docs/FONEPAY-IMPLEMENTATION.md)** - Complete real-time integration guide with code examples

## 🎮 Demo Project

A complete working demo is included showing:
- All payment gateway integrations
- Real-time Fonepay QR payments with SignalR 
- Frontend implementation examples
- Best practices and error handling 

> **Bonus:** Demo project ma sabai kura cha - jasto ki Nepali thali ma sabai tarkari! 🍛

## 🔧 Advanced Features

> NASA ko rocket jasto! 🚀 

### Auto-verification
Fonepay payments are automatically verified when successful - no manual verification needed.

### Timeout Handling
15-minute automatic timeout for QR payments with proper cleanup and user notification.

### Error Management
Comprehensive error handling for network issues, API failures, and timeout scenarios.
### Thread Safety
All WebSocket operations are thread-safe and can handle multiple concurrent payments.
## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
> Gen-Z ko andolan pachi sabai milera desh banam vanne matra ho kasaile gardainan, bidesh maa hune le remittance gardinxan ki vandai basne hun. Bidesh maa basne developer le hamilai manche gandainan

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](docs/LICENSE) file for details.

## 🆘 Support

- **Issues:** [GitHub Issues](https://github.com/shoesheill/Nepal.Payments.Gateways/issues) 
- **Documentation:** See `docs/` folder for detailed guides
- **Demo:** Check the included demo project for examples 

## 🏷️ Version History

- **v1.0.1** - Added Fonepay real-time monitoring and auto-verification 
- **v1.0.0** - Initial release with Khalti and eSewa support 

---

Made with ❤️ for the Nepali developer community

> **Final Note:** Yo package use garera mero maya lagyo vane 1 Cup Milk Tea hai...
