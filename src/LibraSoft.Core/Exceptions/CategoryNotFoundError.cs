namespace LibraSoft.Core.Exceptions
{
    public class CategoryNotFoundError : HandlerError
    {
        public CategoryNotFoundError(string name)
        {
            Errors.Add($"Category {name} do not exists.");
        }
    }
}
