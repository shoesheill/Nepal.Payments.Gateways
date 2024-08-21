using System.Threading.Tasks;

namespace payment_gateway_nepal
{
    public class PaymentManager
    {
        public async Task<T> ProcessPayment<T>(PaymentMethod paymentMethod, PaymentVersion version, string secretKey, PaymentMode paymentMode, object content)
        {
            var paymentService = PaymentServiceFactory.GetPaymentService(paymentMethod, version, secretKey, paymentMode);

            return await paymentService.ProcessPayment<T>(content, version);
        }
        public async Task<T> VerifyPayment<T>(PaymentMethod paymentMethod, PaymentVersion version, PaymentMode paymentMode, string content)
        {
            var paymentService = PaymentServiceFactory.GetPaymentService(paymentMethod, version, "", paymentMode);

            return await paymentService.VerifyPayment<T>(content, version);
        }
    }
}
