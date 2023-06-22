using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using MagicVilla_Utility;

namespace MagicVilla_Web.Services
{
	public class BaseService : IBaseService
	{
		public APIResponse responseModel { get; set; }
		public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
			this.responseModel = new();
			this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
		{
			try
			{
				var client = httpClient.CreateClient("MagicAPI");
				HttpRequestMessage message = new HttpRequestMessage();
				message.Headers.Add("Accept", "application/json");
				message.RequestUri = new Uri(apiRequest.Url);
				if (apiRequest.Data != null)
				{
					message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),Encoding.UTF8, "application/json");
						
				}
				switch (apiRequest.ApiType)
				{
					case SD.ApiType.POST:
						message.Method = HttpMethod.Post;
						break;
					case SD.ApiType.PUT:
						message.Method = HttpMethod.Put;
						break;
					case SD.ApiType.DELETE:
						message.Method = HttpMethod.Delete;
						break;
					default:
						message.Method = HttpMethod.Get;
						break;
				}

				HttpResponseMessage apiResponse = null;
				apiResponse = await client.SendAsync(message);

				var apicontent = await apiResponse.Content.ReadAsStringAsync();
				var APIresponse = JsonConvert.DeserializeObject<T>(apicontent);
				return APIResponse;

			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
