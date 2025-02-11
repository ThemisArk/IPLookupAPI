using IPLookupAPI.Models;
using IPLookupAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http;

namespace IPLookupAPI.Services
{
    public class GetFromIp2cApi : IGetFromIp2cApi
    {
        public readonly HttpClient _httpClient;

        public GetFromIp2cApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IpAddressInfo?> GetFromIp2c(string ip)
        {
            var url = $"https://ip2c.org/{ip}";
            var response = await _httpClient.GetStringAsync(url);
            var responseParts = response.Split(';');

            if(responseParts.Length < 4)
            {
                return null;
            }

            var ipInfo = new IpAddressInfo
            {
                Ip = ip,
                TwoLetterCode = responseParts[1],
                ThreeLetterCode = responseParts[2],
                CountryName = responseParts[3]
            };

            return ipInfo;
        }
    }
}
