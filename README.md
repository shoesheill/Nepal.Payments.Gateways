# Nepal.Payments.Gateways

[![NuGet Version](https://img.shields.io/nuget/v/Nepal.Payments.Gateways.svg)](https://www.nuget.org/packages/Nepal.Payments.Gateways/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET Standard](https://img.shields.io/badge/.NET%20Standard-2.1-blue.svg)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)

A comprehensive .NET library for integrating popular Nepali payment gateways including eSewa and Khalti.

## Quick Start

### Installation

```bash
dotnet add package Nepal.Payments.Gateways
```

### Basic Usage

```csharp
using Nepal.Payments.Gateways;

var paymentManager = new PaymentManager(
    paymentMethod: PaymentMethod.Esewa,
    paymentVersion: PaymentVersion.V2,
    paymentMode: PaymentMode.Sandbox,
    secretKey: "your-secret-key-here"
);
```

## Documentation

📚 **Complete documentation is available in the [`docs/`](docs/) folder:**

- **[Integration Guide](docs/INTEGRATION_GUIDE.md)** - Step-by-step integration instructions

## Supported Gateways

| Gateway | V1 | V2 | Status |
|---------|----|----|--------|
| eSewa   | ✅ | ✅ | Fully Supported |
| Khalti  | ⚠️ | ✅ | V2 Fully Supported, V1 Not Implemented |
| IMEPay  | ❌ | ❌ | Planned |
| FonePay | ❌ | ❌ | Planned |

## Features

- 🏦 **Multiple Payment Gateways**: Support for eSewa and Khalti
- 🔄 **Version Support**: Support for multiple API versions (V1, V2)
- 🧪 **Environment Support**: Both sandbox and production environments
- 🔒 **Secure**: Built-in HMAC SHA256 signature generation
- 📚 **Well Documented**: Comprehensive documentation and examples
- 🎯 **Type Safe**: Strongly typed models and responses
- ⚡ **Async/Await**: Full async support for better performance
- 🛡️ **Error Handling**: Comprehensive error handling and validation

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 📧 Email: shoesheill@gmail.com
- 🐛 Issues: [GitHub Issues](https://github.com/shoesheill/Nepal.Payments.Gateways/issues)
- 📖 Documentation: [docs/](docs/)

---

Made with ❤️ for the Nepali developer community