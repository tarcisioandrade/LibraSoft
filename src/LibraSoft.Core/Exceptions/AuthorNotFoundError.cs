using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class AuthorNotFoundError : ErrorBase
    {

        public AuthorNotFoundError(string name)
        {
            Errors.Add($"Author {name} do no exists.");
        }

        public AuthorNotFoundError()
        {
            Errors.Add("Author do no exists.");
        }
    }
}
