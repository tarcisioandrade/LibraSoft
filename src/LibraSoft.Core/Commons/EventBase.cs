namespace LibraSoft.Core.Commons
{
    public abstract class EventBase
    {
        public abstract Task Execute();

        public void ExecuteSync()
        {
            Execute().Wait();
        }
    }
}
