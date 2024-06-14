using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class CategoryNotFoundError : ErrorBase
    {
        public CategoryNotFoundError(string name)
        {
            Errors.Add($"Category {name} do not exists.");
        }
    }
}
