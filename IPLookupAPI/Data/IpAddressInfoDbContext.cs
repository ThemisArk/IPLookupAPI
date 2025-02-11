using IPLookupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IPLookupAPI.Data
{
    public class IpAddressInfoDbContext : DbContext
    {
        public IpAddressInfoDbContext(DbContextOptions<IpAddressInfoDbContext> options)
            : base(options) { }

        public DbSet<IpAddressInfo> IpAddressInfos { get; set; }
    }
}
