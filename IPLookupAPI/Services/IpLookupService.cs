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

        public IpLookupService(IpAddressInfoDbContext context, IMemoryCache cache, HttpClient httpClient )
        {
            _context = context;
            _cache = cache;
            _httpClient = httpClient;
        }

        public Task<IpAddressInfo> GetIpInformation(string ip)
        {
            throw new NotImplementedException();
        }
    }
}
