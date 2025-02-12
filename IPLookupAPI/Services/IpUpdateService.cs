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

        public IpUpdateService(IMemoryCache cache,
                               HttpClient httpClient,
                               IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _httpClient = httpClient;
            _scopeFactory = scopeFactory;
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
            //inject dbcontext(=scoped) to the background service(=singleton)
            var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IpAddressInfoDbContext>();

            //inject getfromip2capi(=scoped) to the background service(=singleton)
            var callIp2c = scope.ServiceProvider.GetRequiredService<IGetFromIp2cApi>();

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
                    var updatedInfo = await callIp2c.GetFromIp2c(ipInfo.Ip);

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
