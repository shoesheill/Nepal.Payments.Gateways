using Nepal.Payments.Gateways.Helper;
using Xunit;

namespace Nepal.Payments.Gateways.Tests;

/// <summary>
///     Locks GenerateHmacSha512 to lowercase hex output using the worked examples from
///     Fonepay's own Billing-3rdparty-Integration-Requirement.pdf — the digest previously
///     came out base64-encoded, which Fonepay rejects with "Data Validation Failed" (406).
/// </summary>
public class HmacHelperTests
{
    private const string SecretKey = "a7e3512f5032480a83137793cb2021dc";

    [Fact]
    public void GenerateHmacSha512_QrRequestWithoutTaxRefund_MatchesFonepaySample()
    {
        var message = "14,5d76d323-d1f6-4a38-8231-0063f9581c98,NBQM,test1,test2";

        var signature = HmacHelper.GenerateHmacSha512(message, SecretKey);

        Assert.Equal(
            "43d2f0939e58e038c3122cc1e65f86af01998dce3e9f70a41a664dc0dbd45dfd74b4c4cbb77afef8a5ae9854ab48fcbd7edfc93156f663a8c60f28830eaca7d7",
            signature);
    }

    [Fact]
    public void GenerateHmacSha512_CheckQrStatus_MatchesFonepaySample()
    {
        var message = "5d76d323-d1f6-4a38-8231-0063f9581c98,NBQM";

        var signature = HmacHelper.GenerateHmacSha512(message, SecretKey);

        Assert.Equal(
            "de5fd3bbbd7d36c766a47c0a137e41de7587028d2f6e3deacb5bebe30992326876a6fba4f9ccfd55a1d302a81aba94733d6c1db04f749483be63b619a9b032b7",
            signature);
    }

    [Fact]
    public void GenerateHmacSha512_TaxRefund_MatchesFonepaySample()
    {
        var message = "35132,e85d2ae7-e342-4a1c-81d7-536867a6720e,IN2_e85d2ae7-e342-4a1c-81d7-536867a6720e,2076.09.29,14,NBQM";

        var signature = HmacHelper.GenerateHmacSha512(message, SecretKey);

        Assert.Equal(
            "4b3fbd3dbfdf2e6d5a2999e8ace63e3b47153dd91c4c17846ef473b12211b0df4acc9e9dc4f814530880f977f665bccc7f1555310918a1d0ff2a57097b4eb8a4",
            signature);
    }

    [Fact]
    public void GenerateHmacSha256Signature_StaysBase64_EsewaUnaffected()
    {
        // Guards against "fixing" this one too — eSewa's real spec wants base64, unlike
        // Fonepay's hex. Regression check only; not a Fonepay-documented value.
        var signature = HmacHelper.GenerateHmacSha256Signature("total_amount=100,transaction_uuid=abc,product_code=EPAYTEST", "secret");

        Assert.Matches("^[A-Za-z0-9+/]+={0,2}$", signature);
    }
}
