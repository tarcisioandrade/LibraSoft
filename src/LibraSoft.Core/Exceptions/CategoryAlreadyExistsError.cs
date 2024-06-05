using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class CategoryAlreadyExistsError : ErrorBase
    {
        public CategoryAlreadyExistsError()
        {
            Errors.Add("Category already exists.");
        }
    }
}
