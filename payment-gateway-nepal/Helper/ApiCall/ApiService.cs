using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace payment_gateway_nepal
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsyncResult<T>(string apiPath, HttpMethod httpMethod, Dictionary<string, string> headerParam, Dictionary<string, string> keyValuePairs, object? requestBody = null)
        {
            try
            {
                // Set up the request headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headerParam != null)
                    foreach (var keyValue in headerParam)
                    {
                        _httpClient.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                    }

                // Create the request
                var request = new HttpRequestMessage(httpMethod, apiPath)
                {
                    Content = requestBody == null ? null : new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
                };
                if (keyValuePairs != null)
                    request.Content = new FormUrlEncodedContent(keyValuePairs);

                // Send the request
                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    if (response.Content.Headers.ContentType.MediaType == "text/html")
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return (T)Convert.ChangeType(response.RequestMessage.RequestUri.AbsoluteUri, typeof(T));
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(responseBody);
                    }
                }
                else
                {
                    throw new Exception($"Failed to retrieve data. Status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                throw; // Re-throw the exception to be handled by the caller if needed
            }
            catch (JsonException jsonEx)
            {
                throw; // Re-throw the exception to be handled by the caller if needed
            }
            catch (Exception ex)
            {
                throw; // Re-throw the exception to be handled by the caller if needed
            }
        }
    }
}
