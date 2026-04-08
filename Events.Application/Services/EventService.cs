using Events.Application.Extensions;
using Events.Application.Models.Dtos;
using Events.Application.Services.Interfaces;
using Events.Domain.Entities;
using Events.Domain.Repositories;

namespace Events.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(
            IEventRepository repository)
        {
            _repository = repository;
        }

        public EventDto GetEvent(Guid id)
        {
            var @event = _repository.Get(id);

            return new EventDto(
                @event.Id,
                @event.Title,
                @event.Description,
                @event.StartAt,
                @event.EndAt);
        }

        public IReadOnlyCollection<EventDto> GetEvents(GetEventsDto dto)
        {
            var events = _repository.Get(
                dto.Title,
                dto.From,
                dto.To);

            return events.Select(x =>
                new EventDto(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.StartAt,
                    x.EndAt))
                .ToList();
        }

        public Guid CreateEvent(CreateEventDto dto)
        {
            dto.ValidateOrThrow();

            var @event = new Event(
                    dto.Title,
                    dto.Description,
                    dto.StartAt,
                    dto.EndAt);

            _repository.Save(@event);

            return @event.Id;
        }

        public void UpdateEvent(Guid id, UpdateEventDto dto)
        {
            dto.ValidateOrThrow();

            _repository.Update(
                id,
                dto.Title,
                dto.Description,
                dto.StartAt,
                dto.EndAt);
        }

        public void DeleteEvent(Guid id)
        {
            _repository.Delete(id);
        }
    }
}