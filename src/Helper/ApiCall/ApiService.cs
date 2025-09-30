using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Service class for making HTTP API calls to payment gateways.
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Makes an asynchronous HTTP request and returns the deserialized response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to.</typeparam>
        /// <param name="apiPath">The API endpoint URL.</param>
        /// <param name="httpMethod">The HTTP method to use.</param>
        /// <param name="headerParam">Optional headers to include in the request.</param>
        /// <param name="keyValuePairs">Optional form data to include in the request.</param>
        /// <param name="requestBody">Optional JSON body to include in the request.</param>
        /// <returns>A task that represents the asynchronous operation and contains the deserialized response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when apiPath or httpMethod is null.</exception>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
        /// <exception cref="JsonException">Thrown when JSON deserialization fails.</exception>
        public async Task<T> GetAsyncResult<T>(string apiPath, HttpMethod httpMethod, Dictionary<string, string> headerParam = null, Dictionary<string, string> keyValuePairs = null, object requestBody = null)
        {
            if (string.IsNullOrEmpty(apiPath))
                throw new ArgumentNullException(nameof(apiPath), "API path cannot be null or empty.");
            
            if (httpMethod == null)
                throw new ArgumentNullException(nameof(httpMethod), "HTTP method cannot be null.");

            try
            {
                // Set up the request headers
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                if (headerParam != null)
                {
                    foreach (var keyValue in headerParam)
                    {
                        _httpClient.DefaultRequestHeaders.Add(keyValue.Key, keyValue.Value);
                    }
                }

                // Create the request
                var request = new HttpRequestMessage(httpMethod, apiPath);
                
                // Set request content based on what's provided
                if (keyValuePairs != null)
                {
                    request.Content = new FormUrlEncodedContent(keyValuePairs);
                }
                else if (requestBody != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                }

                // Send the request
                var response = await _httpClient.SendAsync(request);
                
                if (response.IsSuccessStatusCode)
                {
                    var contentType = response.Content.Headers.ContentType?.MediaType;
                    
                    if (contentType == "text/html")
                    {
                        // For HTML responses (like redirect URLs), return the request URI as string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return (T)Convert.ChangeType(response.RequestMessage.RequestUri.AbsoluteUri, typeof(T));
                    }
                    else
                    {
                        // For JSON responses, deserialize the content
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<T>(responseBody);
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}. Response: {errorContent}");
                }
            }
            catch (HttpRequestException)
            {
                throw; // Re-throw HTTP exceptions
            }
            catch (JsonException)
            {
                throw; // Re-throw JSON exceptions
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An unexpected error occurred while making the API request: {ex.Message}", ex);
            }
        }
    }
}
