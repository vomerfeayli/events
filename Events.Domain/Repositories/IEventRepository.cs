using Events.Domain.Entities;

namespace Events.Domain.Repositories
{
    public interface IEventRepository
    {
        public Event Get(Guid id);

        public IReadOnlyCollection<Event> Get(
            string title,
            DateTime? from,
            DateTime? to);

        public void Save(Event @event);

        public void Update(
            Guid id,
            string title,
            string description,
            DateTime startAt,
            DateTime endAt);

        public void Delete(Guid id);
    }
}