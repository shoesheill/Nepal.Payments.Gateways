# Nepal.Payments.Gateways - Complete Integration Guide

This comprehensive guide provides detailed instructions for integrating eSewa and Khalti payment gateways into your .NET applications.

## Table of Contents

1. [Quick Setup](#quick-setup)
2. [Architecture Overview](#architecture-overview)
3. [eSewa Integration](#esewa-integration)
4. [Khalti Integration](#khalti-integration)
5. [Fonepay Integration](#fonepay-integration)
6. [Response Handling](#response-handling)
7. [Test Credentials](#test-credentials)
8. [Error Handling](#error-handling)
9. [Best Practices](#best-practices)
10. [Troubleshooting](#troubleshooting)

## Quick Setup

### 1. Install the Package

```bash
dotnet add package Nepal.Payments.Gateways
```

### 2. Basic Configuration

```csharp
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Enum;
using Newtonsoft.Json;

// Configure your payment manager
var paymentManager = new PaymentManager(
    paymentMethod: PaymentMethod.Esewa, // or PaymentMethod.Khalti
    paymentVersion: PaymentVersion.V2,
    paymentMode: PaymentMode.Sandbox, // Use Production for live transactions
    secretKey: "your-secret-key-here"
);
```

## Architecture Overview

### Current Implementation Structure

The library follows a factory pattern with service abstraction:

```
PaymentManager
    ↓
PaymentServiceFactory
    ↓
IPaymentService (V1/V2 implementations)
    ↓
PaymentEndpointFactory → ApiService → HTTP Client
```

### Key Components

- **PaymentManager**: Main entry point for all payment operations
- **PaymentServiceFactory**: Creates appropriate service instances based on method and version
- **IPaymentService**: Interface implemented by eSewa, Khalti, and Fonepay services
- **PaymentEndpointFactory**: Manages API endpoints for different environments
- **ResponseConverter**: Handles safe type conversions
- **ApiService**: HTTP client wrapper with error handling

### Supported Versions

- **eSewa**: V1, V2
- **Khalti**: V1, V2
- **Fonepay**: No versioning (QR-based system)
- **Environments**: Sandbox, Production

## eSewa Integration

### Sandbox Configuration

```csharp
private readonly string eSewa_SecretKey = "8gBm/:&EnhH.1/q";
private readonly string eSewa_MerchantId = "EPAYTEST";
```

### Complete Payment Flow

#### 1. Payment Initialization

```csharp
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Models.eSewa;

public async Task<IActionResult> PayWitheSewa()
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Esewa,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        eSewa_SecretKey
    );

    string currentUrl = new Uri($"{Request.Scheme}://{Request.Host}").AbsoluteUri;

    var request = new PaymentRequest
    {
        Amount = "100",
        TaxAmount = "10",
        TotalAmount = "110",
        TransactionUuid = "bk-" + new Random().Next(10000, 100000).ToString(),
        ProductCode = "EPAYTEST",
        ProductServiceCharge = "0",
        ProductDeliveryCharge = "0",
        SuccessUrl = currentUrl,
        FailureUrl = currentUrl,
        SignedFieldNames = "total_amount,transaction_uuid,product_code"
    };

    var response = await paymentManager.InitiatePaymentAsync<PaymentResult>(request);
    return Redirect(response.Data.ToString());
}
```

#### 2. Payment Verification

```csharp
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Models.eSewa;

public async Task<IActionResult> VerifyEsewaPayment(string data)
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Esewa,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        eSewa_SecretKey
    );

    var response = await paymentManager.VerifyPaymentAsync<PaymentResponse>(data);

    if (!string.IsNullOrEmpty(response.Status) && 
        string.Equals(response.Status, "complete", StringComparison.OrdinalIgnoreCase))
    {
        ViewBag.Message = $"Payment with eSewa completed successfully with data: {response.TransactionCode} and amount: {response.TotalAmount}";
    }
    else
    {
        ViewBag.Message = "Payment with eSewa failed";
    }
    return View();
}
```


## Khalti Integration

### Sandbox Configuration

```csharp
private readonly string Khalti_SecretKey = "live_secret_key_68791341fdd94846a146f0457ff7b455";
```

### Complete Payment Flow

#### 1. Payment Initialization

```csharp
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Models.Khalti;
using Newtonsoft.Json;

public async Task<ActionResult> PayWithKhalti()
{
    string currentUrl = new Uri($"{Request.Scheme}://{Request.Host}").AbsoluteUri;
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Khalti,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        Khalti_SecretKey
    );

    var request = new
    {
        return_url = currentUrl,
        website_url = currentUrl,
        amount = 1300, // Amount in paisa (13 NPR)
        purchase_order_id = "test12",
        purchase_order_name = "test",
        customer_info = new KhaltiCustomerInfo()
        {
            Name = "Sushil Shreshta",
            Email = "shoesheill@gmail.com",
            Phone = "9846000027"
        },
        product_details = new List<KhaltiProductDetail>
        {
            new KhaltiProductDetail()
            {
                Identity = "1234567890",
                Name = "Khalti logo",
                TotalPrice = 1300,
                Quantity = 1,
                UnitPrice = 1300
            }
        },
        amount_breakdown = new List<KhaltiAmountBreakdown>
        {
            new KhaltiAmountBreakdown() { Label = "Mark Price", Amount = 1000 },
            new KhaltiAmountBreakdown() { Label = "VAT", Amount = 300 }
        }
    };

    var response = await paymentManager.InitiatePaymentAsync<PaymentResult>(request);
    var khaltiInitResponse = JsonConvert.DeserializeObject<RequestResponse>(JsonConvert.SerializeObject(response.Data));
    return Redirect(khaltiInitResponse.PaymentUrl);
}
```

#### 2. Payment Verification

```csharp
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Enum;

private async Task<ActionResult> VerifyKhaltiPayment(string pidx)
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Khalti,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        Khalti_SecretKey
    );

    var response = await paymentManager.VerifyPaymentAsync<PaymentResult>(pidx);

    if (response.Success && response.Data != null)
    {
        ViewBag.Message = $"Payment with Khalti completed successfully with pidx: {pidx}";
    }
    else
    {
        ViewBag.Message = $"Payment with Khalti failed: {response.Message}";
    }
    return View();
}
```

## Fonepay Integration

Fonepay is a QR-based payment system that uses WebSocket for real-time payment notifications. Unlike eSewa and Khalti, Fonepay doesn't use redirect-based payments but generates QR codes for customers to scan.

### Sandbox Configuration

```csharp
private readonly string Fonepay_SecretKey = "your-fonepay-secret-key";
private readonly string Fonepay_MerchantCode = "your-merchant-code";
private readonly string Fonepay_Username = "your-username";
private readonly string Fonepay_Password = "your-password";
```

### Complete Payment Flow

#### 1. QR Code Generation

```csharp
using Nepal.Payments.Gateways;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Models.Fonepay;

public async Task<IActionResult> GenerateFonepayQR()
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.FonePay,
        PaymentVersion.V1, // Any version works - Fonepay ignores versioning
        PaymentMode.Sandbox,
        Fonepay_SecretKey
    );

    var request = new QrRequest
    {
        Amount = "14.00",
        Remarks1 = "Test Payment",
        Remarks2 = "QR Integration",
        Prn = Guid.NewGuid().ToString(), // Unique Product Reference Number
        MerchantCode = Fonepay_MerchantCode,
        Username = Fonepay_Username,
        Password = Fonepay_Password,
        TaxAmount = "2.00", // Optional
        TaxRefund = "0.00"  // Optional
    };

    var response = await paymentManager.InitiatePaymentAsync<PaymentResult>(request);
    var qrResponse = JsonConvert.DeserializeObject<QrResponse>(JsonConvert.SerializeObject(response.Data));
    
    if (qrResponse.Success)
    {
        // Store QR message and WebSocket URL for real-time updates
        ViewBag.QrMessage = qrResponse.QrMessage;
        ViewBag.WebSocketUrl = qrResponse.ThirdpartyQrWebSocketUrl;
        ViewBag.Prn = request.Prn;
    }
    
    return View();
}
```

#### 2. Payment Status Check

```csharp
public async Task<IActionResult> CheckFonepayStatus(string prn)
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.FonePay,
        PaymentVersion.V1, // Any version works - Fonepay ignores versioning
        PaymentMode.Sandbox,
        Fonepay_SecretKey
    );

    var verificationData = JsonConvert.SerializeObject(new
    {
        prn = prn,
        merchantCode = Fonepay_MerchantCode,
        username = Fonepay_Username,
        password = Fonepay_Password
    });

    var response = await paymentManager.VerifyPaymentAsync<PaymentResult>(verificationData);
    var statusResponse = JsonConvert.DeserializeObject<QrStatusResponse>(JsonConvert.SerializeObject(response.Data));

    return Json(new
    {
        success = response.Success,
        paymentStatus = statusResponse.PaymentStatus,
        fonepayTraceId = statusResponse.FonepayTraceId
    });
}
```

#### 3. WebSocket Integration for Real-time Updates

See standalone sample: `docs/FONEPAY_WEBSOCKET_SAMPLE.md`.

#### 4. Tax Refund Processing (After Payment)

```csharp
public async Task<IActionResult> ProcessTaxRefund(long fonepayTraceId, string prn, string invoiceNumber)
{
    var paymentService = new Services.Fonepay.PaymentService(Fonepay_SecretKey, PaymentMode.Sandbox);
    
    var taxRefundRequest = new TaxRefundRequest
    {
        FonepayTraceId = fonepayTraceId,
        TransactionAmount = "14.00",
        MerchantPRN = prn,
        InvoiceNumber = invoiceNumber,
        InvoiceDate = DateTime.Now.ToString("yyyy.MM.dd"), // Nepali date format
        MerchantCode = Fonepay_MerchantCode,
        Username = Fonepay_Username,
        Password = Fonepay_Password
    };

    var response = await paymentService.ProcessTaxRefundAsync<PaymentResult>(taxRefundRequest);
    var taxRefundResponse = JsonConvert.DeserializeObject<TaxRefundResponse>(JsonConvert.SerializeObject(response.Data));

    return Json(new
    {
        success = response.Success,
        message = response.Message,
        fonepayTraceId = taxRefundResponse.FonepayTraceId
    });
}
```

## Test Credentials

### eSewa Test Account
- **Username**: `9806800001/2/3/4/5`
- **Password**: `Nepal@123`
- **Token**: `123456`
- **Merchant ID**: `EPAYTEST`
- **Secret Key**: `8gBm/:&EnhH.1/q`

### Khalti Test Account
- **Mobile Number**: `9800000001/2/3/4/5`
- **Pin**: `1111`
- **OTP**: `987654`
- **Secret Key**: `live_secret_key_68791341fdd94846a146f0457ff7b455`

### Fonepay Test Account
- **Merchant ID**: `FONEPAY_TEST_MERCHANT`
- **Secret Key**: `test_secret_key_fonepay_12345`
- **Test Mobile**: `9800000001/2/3/4/5`
- **Test Amount**: `100.00 NPR`

## Error Handling


### Response Types

- **PaymentResult**: Standard wrapper for all responses with Success, Message, Data properties
- **PaymentRequest**: eSewa payment request model (all string properties)
- **PaymentResponse**: eSewa payment verification response
- **RequestResponse**: Khalti payment initiation response
- **Fonepay PaymentRequest**: Fonepay payment request model with merchant details
- **Fonepay PaymentResponse**: Fonepay payment verification response
- **Fonepay RequestResponse**: Fonepay payment initiation response

## Best Practices

### 1. Environment Configuration

```csharp
using Nepal.Payments.Gateways.Enum;

// Use environment variables for sensitive data
private readonly string eSewaSecretKey = Environment.GetEnvironmentVariable("ESEWA_SECRET_KEY");
private readonly string khaltiSecretKey = Environment.GetEnvironmentVariable("KHALTI_SECRET_KEY");
private readonly string fonepaySecretKey = Environment.GetEnvironmentVariable("FONEPAY_SECRET_KEY");
private readonly string fonepayMerchantId = Environment.GetEnvironmentVariable("FONEPAY_MERCHANT_ID");
private readonly PaymentMode paymentMode = Environment.GetEnvironmentVariable("PAYMENT_MODE") == "Production" 
    ? PaymentMode.Production 
    : PaymentMode.Sandbox;
```


## Production Deployment

### 1. Environment Variables

```bash
# Production environment variables
ESEWA_SECRET_KEY=your-production-secret-key
KHALTI_SECRET_KEY=your-production-secret-key
FONEPAY_SECRET_KEY=your-production-secret-key
FONEPAY_MERCHANT_ID=your-production-merchant-id
PAYMENT_MODE=Production
```

### 2. Security Considerations

- Never commit secret keys to version control
- Use HTTPS for all callback URLs
- Implement proper input validation
- Use environment-specific configurations
- Monitor payment transactions
- Implement proper logging and auditing

### 3. Performance Optimization

- Use connection pooling for HTTP clients
- Implement caching for frequently accessed data
- Use async/await properly
- Monitor API response times
- Implement circuit breaker pattern for resilience

## Support and Resources

- **eSewa Developer Guide**: https://developer.esewa.com.np/pages/Introduction
- **Khalti Developer Guide**: https://docs.khalti.com/khalti-epayment/
- **GitHub Repository**: https://github.com/shoesheill/Nepal.Payments.Gateways
- **Issues**: https://github.com/shoesheill/Nepal.Payments.Gateways/issues

---

For additional support or questions, please open an issue on GitHub or contact the maintainers.
