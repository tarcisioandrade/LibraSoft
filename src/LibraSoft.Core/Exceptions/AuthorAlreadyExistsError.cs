namespace LibraSoft.Core.Exceptions
{
    public class AuthorAlreadyExistsError : HandlerError
    {
        public AuthorAlreadyExistsError(string name)
        {
            Errors.Add($"Author {name} already exists.");
        }
    }
}
