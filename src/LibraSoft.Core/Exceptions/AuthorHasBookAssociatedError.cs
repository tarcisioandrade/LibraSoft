using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class AuthorHasBookAssociatedError : ErrorBase
    {

        public AuthorHasBookAssociatedError()
        {
            Errors.Add("The author has associated books, please consider inactive it.");
        }

        public AuthorHasBookAssociatedError(string name)
        {
            Errors.Add($"The author {name} has associated books, please consider inactive it.");
        }
    }
}
