using IPLookupAPI.Models;
using IPLookupAPI.Services.Interfaces;

namespace IPLookupAPI.Services
{
    public class IpLookupService : IIpLookupService
    {
        public Task<IpAddressInfo> GetIpInformation(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
