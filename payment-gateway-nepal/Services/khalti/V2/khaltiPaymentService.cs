using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace payment_gateway_nepal.khalti.V2
{
	internal class KhaltiPaymentService : IPaymentService
	{
		private readonly string _secretKey;
		private readonly PaymentMode _paymentMode;


		public KhaltiPaymentService(string secretKey, PaymentMode paymentMode)
		{
			_secretKey = secretKey;
			_paymentMode = paymentMode;
		}
		public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
		{
			var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.ProcessPayment, _paymentMode);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", "key " + _secretKey);
			// string json = JsonConvert.SerializeObject(content);
			var response = await new ApiService(new HttpClient()).GetAsyncResult<KhaltiInitResponse>(apiUrl, httpMethod, headers, null, content);
			ApiResponse apiResponse = new ApiResponse { data = response };
			return (T)Convert.ChangeType(apiResponse, typeof(T));
		}

		public async Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
		{
			var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.VerifyPayment, _paymentMode);
			Dictionary<string, string> headers = new Dictionary<string, string>();
			headers.Add("Authorization", "key " + _secretKey);
			var formContent = new Dictionary<string, string>
			{
				{ "pidx", content }
			};
			KhaltiResponse response = await new ApiService(new HttpClient()).GetAsyncResult<KhaltiResponse>(apiUrl, httpMethod, headers, formContent, null);
			response.status = response.status;
			return (T)Convert.ChangeType(response, typeof(T));
		}
	}
}
