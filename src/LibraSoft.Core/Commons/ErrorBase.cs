namespace LibraSoft.Core.Commons
{
    public class ErrorBase : SystemException
    {
        public List<string> Errors { get; set; } = [];
    }
}
