using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class BookIsbnAlreadyExistsError : ErrorBase
    {

        public BookIsbnAlreadyExistsError(string isbn)
        {
            Errors.Add($"Already exist a book with '{isbn}' Isbn.");
        }
    }
}
