using System.Threading.Tasks;

namespace payment_gateway_nepal
{
    public interface IPaymentService
    {
        Task<T> ProcessPayment<T>(object content, PaymentVersion version);
        Task<T> VerifyPayment<T>(string content, PaymentVersion version);
    }
}
