using IPLookupAPI.Data;
using IPLookupAPI.Services;
using IPLookupAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPLookupAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Add db
            builder.Services.AddDbContext<IpAddressInfoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Add caching
            builder.Services.AddMemoryCache();

            //Add httpclient
            builder.Services.AddHttpClient();

            //Add services
            builder.Services.AddTransient<IGetFromIp2cApi, GetFromIp2cApi>();
            builder.Services.AddTransient<IIpLookupService, IpLookupService>();

            builder.Services.AddControllers();

            var app = builder.Build();

            app.MapControllers();

            app.Run();
        }
    }
}
