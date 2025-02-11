namespace IPLookupAPI.Models
{
    public class IpAddressInfo
    {
        public int Id { get; set; }
        public string? Ip {  get; set; }
        public string? CountryName { get; set; }
        public string? TwoLetterCode { get; set; }
        public string? ThreeLetterCode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
    }
}
