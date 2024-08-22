using System;
using System.Net.Http;

namespace payment_gateway_nepal
{
    public class PaymentEndpointFactory
    {
        public static (string, HttpMethod) GetEndpoint(PaymentMethod paymentMethod, PaymentVersion version, PaymentAction paymentAction, PaymentMode paymentMode)
        {


            if (paymentMethod == PaymentMethod.eSewa)
            {
                string baseUrl = "";
                string methodName = "";
                HttpMethod httpMethod = HttpMethod.Get;
                if (version == PaymentVersion.v2)
                {
                    baseUrl = (paymentMode == PaymentMode.Production) ? ApiEndPoints.eSewa.V2.BaseUrl : ApiEndPoints.eSewa.V2.SandboxBaseUrl;
                    methodName = (paymentAction == PaymentAction.ProcessPayment) ? (ApiEndPoints.eSewa.V2.ProcessPaymentUrl) : (ApiEndPoints.eSewa.V2.VerifyPaymentUrl);
                    httpMethod = paymentAction == PaymentAction.ProcessPayment ? ApiEndPoints.eSewa.V2.ProcessPaymentMethod : ApiEndPoints.eSewa.V2.VerifyPaymentMethod;
                    return (baseUrl + methodName, httpMethod);
                }
            }
            else if (paymentMethod == PaymentMethod.Khalti)
            {
                string baseUrl = "";
                string methodName = "";
                HttpMethod httpMethod = HttpMethod.Get;
                if (version == PaymentVersion.v2)
                {
                    baseUrl = (paymentMode == PaymentMode.Production) ? ApiEndPoints.Khalti.V2.BaseUrl : ApiEndPoints.Khalti.V2.SandboxBaseUrl;
                    methodName = (paymentAction == PaymentAction.ProcessPayment) ? (ApiEndPoints.Khalti.V2.ProcessPaymentUrl) : (ApiEndPoints.Khalti.V2.VerifyPaymentUrl);
                    httpMethod = paymentAction == PaymentAction.ProcessPayment ? ApiEndPoints.Khalti.V2.ProcessPaymentMethod : ApiEndPoints.Khalti.V2.VerifyPaymentMethod;
                    return (baseUrl + methodName, httpMethod);
                }
            }
            return (string.Empty, HttpMethod.Get);
        }
    }
}
