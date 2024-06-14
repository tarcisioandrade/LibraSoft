using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class BookHasRentAssociatedError : ErrorBase
    {
        public BookHasRentAssociatedError()
        {
            Errors.Add("The book has associated rent, please consider inactive it.");
        }

        public BookHasRentAssociatedError(string name)
        {
            Errors.Add($"The book '{name}' has associated rent, please consider inactive it.");
        }
    }
}
