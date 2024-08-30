using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace payment_gateway_nepal.khalti.V2
{
	internal class khaltiPaymentService : IPaymentService
	{
		private readonly string _secretKey;
		private readonly PaymentMode _paymentMode;


		public khaltiPaymentService(string secretKey, PaymentMode paymentMode)
		{
			_secretKey = secretKey;
			_paymentMode = paymentMode;
		}
		public async Task<T> ProcessPayment<T>(object content, PaymentVersion version)
		{
			var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.ProcessPayment, _paymentMode);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", "key " + _secretKey);
			// string json = JsonConvert.SerializeObject(content);
			var response = await new ApiService(new HttpClient()).GetAsyncResult<k_init_response>(apiUrl, httpMethod, headers, null, content);
			ApiResponse apiResponse = new ApiResponse { data = response };
			return (T)Convert.ChangeType(apiResponse, typeof(T));
		}

		public async Task<T> VerifyPayment<T>(string content, PaymentVersion version)
		{
			var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.VerifyPayment, _paymentMode);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", "key " + _secretKey);
			var formContent = new Dictionary<string, string>
			{
				{ "pidx", content }
			};
			k_response response = await new ApiService(new HttpClient()).GetAsyncResult<k_response>(apiUrl, httpMethod, headers, formContent, null);
			response.status = response.status;
			return (T)Convert.ChangeType(response, typeof(T));
		}
	}
}
