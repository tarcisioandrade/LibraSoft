using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class ReviewNotFound : ErrorBase
    {
        public ReviewNotFound()
        {
            this.Errors.Add("Review not found.");
        }
    }
}
