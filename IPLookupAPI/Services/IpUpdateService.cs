using IPLookupAPI.Data;
using IPLookupAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IPLookupAPI.Services
{
    public class IpUpdateService : BackgroundService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly int BatchSize = 100;
        private readonly IGetFromIp2cApi _igetFromIp2cApi;

        public IpUpdateService(IMemoryCache cache,
                               HttpClient httpClient,
                               IServiceScopeFactory scopeFactory,
                               IGetFromIp2cApi getFromIp2CApi)
        {
            _cache = cache;
            _httpClient = httpClient;
            _scopeFactory = scopeFactory;
            _igetFromIp2cApi = getFromIp2CApi;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await UpdateIpInformation();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task UpdateIpInformation()
        {
            var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IpAddressInfoDbContext>();

            int totalCount = await dbContext.IpAddressInfos.CountAsync();
            int processed = 0;

            while (processed < totalCount)
            {
                //Take 100 Ips from Db
                var ipBatch = await dbContext.IpAddressInfos
                    .OrderBy(i => i.Id)
                    .Skip(processed)
                    .Take(BatchSize) //BatchSize=100
                    .ToListAsync();

                foreach (var ipInfo in ipBatch)
                {
                    //Call Ip2c to get updated info
                    var updatedInfo = await _igetFromIp2cApi.GetFromIp2c(ipInfo.Ip);

                    if(updatedInfo != null && ipInfo.CountryName != updatedInfo.CountryName)
                    {
                        ipInfo.CountryName = updatedInfo.CountryName;
                        ipInfo.TwoLetterCode = updatedInfo.TwoLetterCode;
                        ipInfo.ThreeLetterCode = updatedInfo.ThreeLetterCode;

                        _cache.Remove(ipInfo.Ip);
                    }
                }

                await dbContext.SaveChangesAsync();
                processed += BatchSize;

            }
        }
    }
}
