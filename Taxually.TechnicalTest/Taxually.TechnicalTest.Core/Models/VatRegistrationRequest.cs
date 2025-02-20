namespace Taxually.TechnicalTest.Core.Models
{
    public class VatRegistrationRequest
    {
        public CountryCode Country { get; set; }
        public string CompanyName { get; set; }
        public string CompanyId { get; set; }
    }
}
