namespace LibraSoft.Domain.ValueObjects
{
    public class Address
    {
        public string ZipCode { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
