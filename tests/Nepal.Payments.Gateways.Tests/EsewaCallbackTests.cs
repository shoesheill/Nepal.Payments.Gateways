using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Helper;
using Nepal.Payments.Gateways.Manager;
using Nepal.Payments.Gateways.Models;
using Newtonsoft.Json;
using Xunit;

namespace Nepal.Payments.Gateways.Tests;

/// <summary>eSewa V2 redirects back with ?data=base64(json); its signature must actually be checked.</summary>
public class EsewaCallbackTests
{
    private const string SecretKey = "8gBm/:&EnhH.1/q";
    private const string SignedFields = "transaction_code,status,total_amount,transaction_uuid,product_code,signed_field_names";

    private static Dictionary<string, string> Callback() => new()
    {
        ["transaction_code"] = "000AWEO",
        ["status"] = "COMPLETE",
        ["total_amount"] = "1000.0",
        ["transaction_uuid"] = "250610-162413",
        ["product_code"] = "EPAYTEST",
        ["signed_field_names"] = SignedFields,
    };

    private static string Sign(Dictionary<string, string> fields) =>
        HmacHelper.GenerateHmacSha256Signature(
            string.Join(",", SignedFields.Split(',').Select(f => $"{f}={fields[f]}")), SecretKey);

    private static string Encode(Dictionary<string, string> fields) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fields)));

    private static Task<PaymentResult> Verify(string data) =>
        new PaymentManager(PaymentMethod.Esewa, PaymentVersion.V2, PaymentMode.Sandbox, SecretKey).VerifyPaymentAsync<PaymentResult>(data);

    [Fact]
    public async Task ValidSignature_Verifies()
    {
        var fields = Callback();
        fields["signature"] = Sign(fields);

        var result = await Verify(Encode(fields));

        Assert.True(result.Success, result.Message);
    }

    [Fact]
    public async Task TamperedAmount_IsRejected()
    {
        var fields = Callback();
        fields["signature"] = Sign(fields);
        fields["total_amount"] = "1.0";

        var result = await Verify(Encode(fields));

        Assert.False(result.Success);
    }

    [Fact]
    public async Task MissingSignature_IsRejected()
    {
        var result = await Verify(Encode(Callback()));

        Assert.False(result.Success);
    }
}
