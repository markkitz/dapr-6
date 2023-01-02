
namespace Onboarding.EventPubs;

public interface IEventPub<T>
{        Task PublishEventAsync( T eventData);
}
