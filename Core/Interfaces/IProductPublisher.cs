namespace Core.Interfaces
{
    public interface IProductPublisher
    {
        Task Publish(string message);
    }
}
