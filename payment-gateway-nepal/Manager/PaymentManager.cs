using System.Threading.Tasks;

namespace payment_gateway_nepal
{
	public class PaymentManager
	{
		private readonly string _secretKey = null;
		private readonly PaymentMethod _method;
		private readonly PaymentVersion _version;
		private readonly PaymentMode _mode;
		public PaymentManager(PaymentMethod paymentMethod, PaymentVersion paymentVersion, PaymentMode paymentMode, string secretkey)
		{
			_secretKey = secretkey;
			_method = paymentMethod;
			_version = paymentVersion;
			_mode = paymentMode;
		}
		public async Task<T> ProcessPayment<T>(object content)
		{
			var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode);

			return await paymentService.ProcessPayment<T>(content, _version);
		}
		public async Task<T> VerifyPayment<T>(string content)
		{
			var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode);

			return await paymentService.VerifyPayment<T>(content, _version);
		}
	}
}
