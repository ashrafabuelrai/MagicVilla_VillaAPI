using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static MagicVilla_Utility.SD;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new ApiResponse();
            this.httpClient = httpClient;
        }
        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
           try
            {
                var client = httpClient.CreateClient("MagicVilla");
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.RequestUri = new Uri(apiRequest.Url);
                if(apiRequest.Data!=null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch(apiRequest.ApiType)
                {
                    case ApiType.POST:
                        requestMessage.Method = HttpMethod.Post;
                        break;
                    case ApiType.GET:
                        requestMessage.Method = HttpMethod.Get;
                        break;
                    case ApiType.PUT:
                        requestMessage.Method = HttpMethod.Put;
                        break;
                    default:
                        requestMessage.Method = HttpMethod.Delete;
                        break;

                }
                HttpResponseMessage responseMessage = null;
                if(!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }
                responseMessage = await client.SendAsync(requestMessage);
                var repsoneContent = await responseMessage.Content.ReadAsStringAsync();
                try
                {
                    ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(repsoneContent);
                    if (apiResponse != null && (apiResponse.StatusCode==HttpStatusCode.BadRequest ||
                         apiResponse.StatusCode == HttpStatusCode.NotFound))
                    {
                        apiResponse.StatusCode = HttpStatusCode.BadRequest;
                        apiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(apiResponse);
                        var retunrObj = JsonConvert.DeserializeObject<T>(res);
                        return retunrObj; 

                    }
                }
                catch
                {
                    var apiResponse = JsonConvert.DeserializeObject<T>(repsoneContent);
                    return apiResponse;
                }
                return JsonConvert.DeserializeObject<T>(repsoneContent);

            }
            catch(Exception ex)
            {
                var dto = new ApiResponse()
                {
                    ErrorMessage = new List<string>() { ex.Message },
                    IsSuccess=false
                };
                var res = JsonConvert.SerializeObject(dto);
                return JsonConvert.DeserializeObject<T>(res);
            }
        }
    }
}
