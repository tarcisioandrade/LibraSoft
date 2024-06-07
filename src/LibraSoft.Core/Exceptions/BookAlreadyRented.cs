namespace LibraSoft.Core.Exceptions
{
    public class BookAlreadyRented : ControllerError
    {
        public BookAlreadyRented(Guid id)
        {
            Errors.Add($"The book id '{id}' has already been rented by this user");
        }
    }
}
