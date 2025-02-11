using IPLookupAPI.Models;
using IPLookupAPI.Services.Interfaces;

namespace IPLookupAPI.Services
{
    public class GetFromIp2cApi : IGetFromIp2cApi
    {
        public readonly HttpClient _httpClient;

        public GetFromIp2cApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IpAddressInfo> GetFromIp2c(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
