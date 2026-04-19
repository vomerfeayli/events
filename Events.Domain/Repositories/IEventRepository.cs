using Events.Domain.Entities;

namespace Events.Domain.Repositories
{
    public interface IEventRepository
    {
        public Event Get(Guid id);

        public IReadOnlyCollection<Event> Get(
            int page,
            int pageSize,
            string title = null,
            DateTime? from = null,
            DateTime? to = null);

        public void Save(Event @event);

        public void Update(
            Guid id,
            string title,
            string description,
            DateTime startAt,
            DateTime endAt);

        public void Delete(Guid id);

        public int GetEventsCount(
            string title = null,
            DateTime? from = null,
            DateTime? to = null);
    }
}