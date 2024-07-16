using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class ReviewAlreadyCreated : ErrorBase
    {
        public ReviewAlreadyCreated()
        {
            this.Errors.Add("This user already created a review that book.");
        }
    }
}
