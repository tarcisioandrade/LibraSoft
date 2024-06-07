namespace LibraSoft.Core.Exceptions
{
    public class BookNotFoundError : HandlerError
    {

        public BookNotFoundError(string id)
        {
            Errors.Add($"Book id: '{id}' do not exists.");
        }
    }
}
