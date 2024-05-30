namespace LibraSoft.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string Country { get; private set; } = string.Empty;
        public string ZipCode { get; private set; } = string.Empty;
    }
}
