# Nepal.Payments.Gateways - Complete Integration Guide

This comprehensive guide provides detailed instructions for integrating eSewa and Khalti payment gateways into your .NET applications.

## Table of Contents

1. [Quick Setup](#quick-setup)
2. [eSewa Integration](#esewa-integration)
3. [Khalti Integration](#khalti-integration)
4. [Test Credentials](#test-credentials)
5. [Error Handling](#error-handling)
6. [Best Practices](#best-practices)
7. [Troubleshooting](#troubleshooting)

## Quick Setup

### 1. Install the Package

```bash
dotnet add package Nepal.Payments.Gateways
```

### 2. Basic Configuration

```csharp
using Nepal.Payments.Gateways;

// Configure your payment manager
var paymentManager = new PaymentManager(
    paymentMethod: PaymentMethod.Esewa, // or PaymentMethod.Khalti
    paymentVersion: PaymentVersion.V2,
    paymentMode: PaymentMode.Sandbox, // Use Production for live transactions
    secretKey: "your-secret-key-here"
);
```

## eSewa Integration

### Sandbox Configuration

```csharp
private readonly string eSewa_SecretKey = "8gBm/:&EnhH.1/q";
private readonly string eSewa_MerchantId = "EPAYTEST";
```

### Complete Payment Flow

#### 1. Payment Initialization

```csharp
public async Task<IActionResult> PayWitheSewa()
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Esewa,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        eSewa_SecretKey
    );

    string currentUrl = new Uri($"{Request.Scheme}://{Request.Host}").AbsoluteUri;

    var request = new EsewaRequest
    {
        Amount = 100,
        TaxAmount = 10,
        TotalAmount = 110,
        TransactionUuid = "bk-" + new Random().Next(10000, 100000).ToString(),
        ProductCode = "EPAYTEST",
        ProductServiceCharge = 0,
        ProductDeliveryCharge = 0,
        SuccessUrl = currentUrl,
        FailureUrl = currentUrl,
        SignedFieldNames = "total_amount,transaction_uuid,product_code"
    };

    var response = await paymentManager.InitiatePaymentAsync<ApiResponse>(request);
    return Redirect(response.Data.ToString());
}
```

#### 2. Payment Verification

```csharp
public async Task<IActionResult> VerifyEsewaPayment(string data)
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Esewa,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        eSewa_SecretKey
    );

    var response = await paymentManager.VerifyPaymentAsync<EsewaResponse>(data);

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

### eSewa Request Model

```csharp
public class EsewaRequest
{
    public decimal Amount { get; set; }                    // Base amount
    public decimal TaxAmount { get; set; }                 // Tax amount
    public decimal TotalAmount { get; set; }               // Total amount (Amount + Tax + Charges)
    public string TransactionUuid { get; set; }            // Unique transaction ID
    public string ProductCode { get; set; }                // Product/service code
    public decimal ProductServiceCharge { get; set; }      // Service charge
    public decimal ProductDeliveryCharge { get; set; }     // Delivery charge
    public string SuccessUrl { get; set; }                 // Success callback URL
    public string FailureUrl { get; set; }                 // Failure callback URL
    public string SignedFieldNames { get; set; }           // Fields to sign
    public string Signature { get; set; }                  // Generated signature
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

    var response = await paymentManager.InitiatePaymentAsync<ApiResponse>(request);
    var khaltiInitResponse = JsonConvert.DeserializeObject<KhaltiInitResponse>(JsonConvert.SerializeObject(response.Data));
    return Redirect(khaltiInitResponse.PaymentUrl);
}
```

#### 2. Payment Verification

```csharp
private async Task<ActionResult> VerifyKhaltiPayment(string pidx)
{
    PaymentManager paymentManager = new PaymentManager(
        PaymentMethod.Khalti,
        PaymentVersion.V2,
        PaymentMode.Sandbox,
        Khalti_SecretKey
    );

    var response = await paymentManager.VerifyPaymentAsync<KhaltiResponse>(pidx);

    if (response != null && string.Equals(response.Status, "completed", StringComparison.OrdinalIgnoreCase))
    {
        ViewBag.Message = $"Payment with Khalti completed successfully with pidx: {response.Pidx} and amount: {response.TotalAmount}";
    }
    else
    {
        ViewBag.Message = "Payment with Khalti failed";
    }
    return View();
}
```

### Khalti Models

```csharp
public class KhaltiCustomerInfo
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

public class KhaltiProductDetail
{
    public string Identity { get; set; }
    public string Name { get; set; }
    public int TotalPrice { get; set; }    // Amount in paisa
    public int Quantity { get; set; }
    public int UnitPrice { get; set; }     // Amount in paisa
}

public class KhaltiAmountBreakdown
{
    public string Label { get; set; }
    public int Amount { get; set; }        // Amount in paisa
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

## Error Handling

### Common Error Scenarios

```csharp
try
{
    var response = await paymentManager.InitiatePaymentAsync<ApiResponse>(paymentRequest);
    
    if (response.Success)
    {
        // Handle successful payment initiation
        return Redirect(response.Data.ToString());
    }
    else
    {
        // Handle payment initiation failure
        ViewBag.Error = response.Message;
        return View("Error");
    }
}
catch (ArgumentException ex)
{
    // Handle invalid parameters
    ViewBag.Error = $"Invalid parameter: {ex.Message}";
    return View("Error");
}
catch (HttpRequestException ex)
{
    // Handle network/HTTP errors
    ViewBag.Error = $"Network error: {ex.Message}";
    return View("Error");
}
catch (JsonException ex)
{
    // Handle JSON parsing errors
    ViewBag.Error = $"Data parsing error: {ex.Message}";
    return View("Error");
}
catch (Exception ex)
{
    // Handle unexpected errors
    ViewBag.Error = $"Unexpected error: {ex.Message}";
    return View("Error");
}
```

### Error Response Handling

```csharp
public class ApiResponse : BaseResponse
{
    // Inherits from BaseResponse
}

public abstract class BaseResponse
{
    public HttpStatusCode Status { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
    public bool Success { get; set; }
    public int ErrorCode { get; set; }
}
```

## Best Practices

### 1. Environment Configuration

```csharp
// Use environment variables for sensitive data
private readonly string eSewaSecretKey = Environment.GetEnvironmentVariable("ESEWA_SECRET_KEY");
private readonly string khaltiSecretKey = Environment.GetEnvironmentVariable("KHALTI_SECRET_KEY");
private readonly PaymentMode paymentMode = Environment.GetEnvironmentVariable("PAYMENT_MODE") == "Production" 
    ? PaymentMode.Production 
    : PaymentMode.Sandbox;
```

### 2. Transaction ID Generation

```csharp
// Generate unique transaction IDs
public string GenerateTransactionId(string prefix = "TXN")
{
    return $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..8]}";
}
```

### 3. Amount Validation

```csharp
// Validate amounts before processing
public bool ValidateAmount(decimal amount)
{
    return amount > 0 && amount <= 1000000; // Adjust limits as needed
}
```

### 4. URL Validation

```csharp
// Validate callback URLs
public bool IsValidUrl(string url)
{
    return Uri.TryCreate(url, UriKind.Absolute, out Uri result) 
           && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
}
```

## Troubleshooting

### Common Issues

#### 1. Signature Verification Failed
- **Cause**: Incorrect secret key or signed field names
- **Solution**: Verify secret key and ensure signed field names match exactly

#### 2. Invalid Product Code
- **Cause**: Using wrong product code for environment
- **Solution**: Use `EPAYTEST` for sandbox, your actual product code for production

#### 3. Amount Mismatch
- **Cause**: Total amount doesn't match sum of components
- **Solution**: Ensure `TotalAmount = Amount + TaxAmount + ProductServiceCharge + ProductDeliveryCharge`

#### 4. Network Timeout
- **Cause**: Slow network or gateway unavailability
- **Solution**: Implement retry logic with exponential backoff

### Debug Mode

```csharp
// Enable detailed logging for debugging
public class PaymentManager
{
    private readonly bool _debugMode;
    
    public PaymentManager(/* parameters */, bool debugMode = false)
    {
        _debugMode = debugMode;
    }
    
    private void LogDebug(string message)
    {
        if (_debugMode)
        {
            Console.WriteLine($"[DEBUG] {DateTime.UtcNow}: {message}");
        }
    }
}
```

## Production Deployment

### 1. Environment Variables

```bash
# Production environment variables
ESEWA_SECRET_KEY=your-production-secret-key
KHALTI_SECRET_KEY=your-production-secret-key
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
