using System.Threading.Tasks;

namespace CourseCatalog.Api.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(string eventName, T eventData);
    }
}
