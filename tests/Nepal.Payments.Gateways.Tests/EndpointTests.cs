using System.Net.Http;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Factories;
using Nepal.Payments.Gateways.Models.eSewa;
using Nepal.Payments.Gateways.Services.Esewa.V2;
using Xunit;

namespace Nepal.Payments.Gateways.Tests;

public class EndpointTests
{
    [Theory]
    [InlineData(PaymentMode.Production, "https://epay.esewa.com.np/api/epay/main/v2/form")]
    [InlineData(PaymentMode.Sandbox, "https://rc-epay.esewa.com.np/api/epay/main/v2/form")]
    public void EsewaV2_FormUrl_IsNotDoubled(PaymentMode mode, string expected)
    {
        var (url, method) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Esewa, PaymentVersion.V2, PaymentAction.ProcessPayment, mode);

        Assert.Equal(expected, url);
        Assert.Equal(HttpMethod.Post, method);
    }

    [Theory]
    [InlineData(PaymentMode.Production, PaymentAction.ProcessPayment, "https://khalti.com/api/v2/epayment/initiate/")]
    [InlineData(PaymentMode.Production, PaymentAction.VerifyPayment, "https://khalti.com/api/v2/epayment/lookup/")]
    [InlineData(PaymentMode.Sandbox, PaymentAction.ProcessPayment, "https://dev.khalti.com/api/v2/epayment/initiate/")]
    [InlineData(PaymentMode.Sandbox, PaymentAction.VerifyPayment, "https://dev.khalti.com/api/v2/epayment/lookup/")]
    public void KhaltiV2_UsesTheDocumentedHosts(PaymentMode mode, PaymentAction action, string expected)
    {
        var (url, _) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, PaymentVersion.V2, action, mode);

        Assert.Equal(expected, url);
    }

    [Theory]
    [InlineData("https://proxy.example.com/khalti")]
    [InlineData("https://proxy.example.com/khalti/")]
    public void BaseUrlOverride_ReplacesTheHost(string baseUrl)
    {
        var (url, _) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, PaymentVersion.V2, PaymentAction.VerifyPayment, PaymentMode.Production, baseUrl);

        Assert.Equal("https://proxy.example.com/khalti/epayment/lookup/", url);
    }

    [Fact]
    public void EsewaV2_StatusUrl_CarriesTheThreeQueryFields()
    {
        var url = PaymentService.BuildStatusUrl("https://rc-epay.esewa.com.np/api",
            new StatusRequest { ProductCode = "EPAYTEST", TotalAmount = "100", TransactionUuid = "a b" });

        Assert.Equal("https://rc-epay.esewa.com.np/api/epay/transaction/status/?product_code=EPAYTEST&total_amount=100&transaction_uuid=a%20b", url);
    }
}
