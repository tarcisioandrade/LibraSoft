namespace LibraSoft.Core.Exceptions
{
    public class NoBookCopiesAvaliableError : ControllerError
    {
        public NoBookCopiesAvaliableError(string name)
        {
            Errors.Add($"No copies of the {name} book are available");
        }
    }
}
