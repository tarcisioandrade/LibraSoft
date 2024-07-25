using LibraSoft.Core.Commons;

namespace LibraSoft.Core.Exceptions
{
    public class BagLimitExcedeed : ErrorBase
    {

        public BagLimitExcedeed()
        {
            this.Errors.Add("User Bag limit has excedeed (5).");
        }   
    }
}
