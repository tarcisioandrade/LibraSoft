using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class RentAlreadyCanceled : ErrorBase
    {
        public RentAlreadyCanceled(Guid id)
        {
            Errors.Add($"Rent id '{id}' already canceled.");
        }
    }
}
