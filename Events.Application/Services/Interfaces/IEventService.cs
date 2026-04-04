using Events.Application.Models.Dtos;

namespace Events.Application.Services.Interfaces
{
    public interface IEventService
    {
        IReadOnlyCollection<EventDto> GetEvents();

        EventDto GetEvent(Guid id);

        Guid CreateEvent(CreateEventDto dto);

        void UpdateEvent(Guid id, UpdateEventDto dto);

        void DeleteEvent(Guid id);
    }
}