using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class AuthorNotFound : ErrorBase
    {
        public AuthorNotFound()
        {
            Errors.Add("Author not found.");
        }
    }
}
