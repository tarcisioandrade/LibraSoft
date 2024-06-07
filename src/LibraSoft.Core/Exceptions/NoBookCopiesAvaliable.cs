namespace LibraSoft.Core.Exceptions
{
    public class NoBookCopiesAvaliable : ControllerError
    {
        public NoBookCopiesAvaliable(string name)
        {
            Errors.Add($"No copies of the {name} book are available");
        }
    }
}
