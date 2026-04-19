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

        public PagedResult<EventDto> GetEvents(GetEventsDto dto)
        {
            dto.ValidateOrThrow();

            var events = _repository.Get(
                dto.Page,
                dto.PageSize,
                dto.Title,
                dto.From,
                dto.To);

            var eventsCount = _repository.GetEventsCount(
                dto.Title,
                dto.From,
                dto.To);

            return new PagedResult<EventDto>(
                events
                    .Select(x =>
                        new EventDto(
                            x.Id,
                            x.Title,
                            x.Description,
                            x.StartAt,
                            x.EndAt))
                    .ToList(),
                dto.Page,
                (int)Math.Ceiling((double)eventsCount / dto.PageSize),
                eventsCount);
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