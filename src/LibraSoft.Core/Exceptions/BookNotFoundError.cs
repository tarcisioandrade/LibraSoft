namespace LibraSoft.Core.Exceptions
{
    public class BookNotFoundError : HandlerError
    {

        public BookNotFoundError(Guid id)
        {
            Errors.Add($"Book id: '{id}' do not exists.");
        }
    }
}
