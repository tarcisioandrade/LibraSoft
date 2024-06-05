namespace LibraSoft.Core.Exceptions
{
    public class BookNotFoundError : HandlerError
    {

        public BookNotFoundError(string name)
        {
            Errors.Add($"Book {name} do no exists.");
        }
    }
}
