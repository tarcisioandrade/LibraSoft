namespace LibraSoft.Core.Exceptions
{
    public class AuthorNotFoundError : HandlerError
    {

        public AuthorNotFoundError(string name)
        {
            Errors.Add($"Author {name} do no exists.");
        }
    }
}
