using Events.Domain.Entities;
using Events.Domain.Repositories;

namespace Events.Persistence.Repositories
{
    public class EventsInMemoryRepository : IEventRepository
    {
        private readonly List<Event> _events = [];

        public Event Get(Guid id)
        {
            return _events.FirstOrDefault(x => x.Id == id)
                   ?? throw new KeyNotFoundException($"Event with id {id} not found");
        }

        public IReadOnlyCollection<Event> Get(
            int page,
            int pageSize,
            string title,
            DateTime? from,
            DateTime? to)
        {
            var query = _events.AsEnumerable();

            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(x => x.Title == title);
            }

            if (from != null)
            {
                query = query.Where(x => x.StartAt >= from);
            }

            if (to != null)
            {
                query = query.Where(x => x.EndAt <= to);
            }

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public void Save(Event @event)
        {
            _events.Add(@event);
        }

        public void Update(
            Guid id,
            string title,
            string description,
            DateTime startAt,
            DateTime endAt)
        {
            var existing = _events.FirstOrDefault(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Event with id {id} not found");

            existing.Update(
                title,
                description,
                startAt,
                endAt
            );
        }

        public void Delete(Guid id)
        {
            var @event = _events.FirstOrDefault(x => x.Id == id)
                   ?? throw new KeyNotFoundException($"Event with id {id} not found");

            _events.Remove(@event);
        }

        public int GetEventsCount()
            => _events.Count;
    }
}