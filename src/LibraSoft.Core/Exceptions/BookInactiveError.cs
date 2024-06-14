using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class BookInactiveError : ErrorBase
    {
        public BookInactiveError()
        {
            Errors.Add("It is not allowed to create a rent from an inactive book");
        }
    }
}
