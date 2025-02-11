using IPLookupAPI.Data;
using IPLookupAPI.Models;
using IPLookupAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IPLookupAPI.Services
{
    public class IpLookupService : IIpLookupService
    {
        public readonly IpAddressInfoDbContext _context;
        public readonly IMemoryCache _cache;
        public readonly HttpClient _httpClient;
        public readonly IGetFromIp2cApi _igetFromIp2cApi;

        public IpLookupService(IpAddressInfoDbContext context, 
                               IMemoryCache cache, 
                               HttpClient httpClient,
                               IGetFromIp2cApi getFromIp2CApi)
        {
            _context = context;
            _cache = cache;
            _httpClient = httpClient;
            _igetFromIp2cApi = getFromIp2CApi;
        }

        public async Task<IpAddressInfo?> GetIpInformation(string ip)
        {
            //check cache
            if (_cache.TryGetValue(ip, out IpAddressInfo? cachedIpInfo))
            {
                return cachedIpInfo;
            }

            //check Db
            var dbIpInfo = await _context.IpAddressInfos
                .FirstOrDefaultAsync(i => i.Ip == ip);
            if( dbIpInfo != null )
            {
                _cache.Set(ip, dbIpInfo);
                return dbIpInfo;
            }

            //call ip2c
            var newIpInfo = await _igetFromIp2cApi.GetFromIp2c(ip);
            if ( newIpInfo != null ) 
            {
                //save in Db
                _context.Add(newIpInfo);
                await _context.SaveChangesAsync();

                //store in cache
                _cache.Set(ip, newIpInfo);
            }

            return newIpInfo;
        }
    }
}
