using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace Converter.Wpf
{
    public class ApiResponse
    {
        public string Error { get; set; }

        public string Result { get; set; }
    }

    public interface IConverterService
    {
        Task<ApiResponse> SumToWordsAsync(string value);
    }

    internal class ConverterService : IConverterService
    {
        public async Task<ApiResponse> SumToWordsAsync(string value)
        {
            await Task.Yield();

            var baseUri = ConfigurationManager.AppSettings["baseUri"] ?? throw new ArgumentException($"App config baseUri' was not found");
            using (var client = new HttpClient()) {
                var response = await client.GetAsync(new Uri($"{baseUri}/{Uri.EscapeDataString(value)}"));
                return response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest ?
                    JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync()) :
                    new ApiResponse { Error = $"Request failed with {response.StatusCode} status" };
            }
        }
    }
}
