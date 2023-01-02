using OfferService.Models;
namespace OfferService.EventPubs;

public interface IEventPub
{
        Task PublishEventAsync<T>( T eventData);
}
