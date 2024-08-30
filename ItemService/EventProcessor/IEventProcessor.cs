namespace ItemService.EventProcessor
{
    public interface IEventProcessor
    {
        void Process(string message);
    }
}
