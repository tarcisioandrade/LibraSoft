using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class RentNotFoundError : ErrorBase
    {
        public RentNotFoundError(Guid id)
        {
            Errors.Add($"Rent id '{id}' not found.");
        }
    }
}
