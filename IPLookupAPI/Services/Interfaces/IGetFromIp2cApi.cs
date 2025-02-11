using IPLookupAPI.Models;

namespace IPLookupAPI.Services.Interfaces
{
    public interface IGetFromIp2cApi
    {
        public Task<IpAddressInfo> GetFromIp2c(string ip);
    }
}
