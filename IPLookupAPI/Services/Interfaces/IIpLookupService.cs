using IPLookupAPI.Models;

namespace IPLookupAPI.Services.Interfaces
{
    public interface IIpLookupService
    {
        public Task<IpAddressInfo?> GetIpInformation(string ip);
    }
}
