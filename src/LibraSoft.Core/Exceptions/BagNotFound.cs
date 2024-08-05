using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class BagNotFound : ErrorBase
    {

        public BagNotFound()
        {
            this.Errors.Add("Bag not found.");
        }   
    }
}
