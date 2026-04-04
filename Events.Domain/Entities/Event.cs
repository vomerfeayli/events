namespace Events.Domain.Entities
{
    public class Event
    {
        public Event(
            string title,
            string description,
            DateTime startAt,
            DateTime endAt)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StartAt = startAt;
            EndAt = endAt;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime StartAt { get; private set; }
        public DateTime EndAt { get; private set; }

        public void Update(
            string title,
            string description,
            DateTime startAt,
            DateTime endAt)
        {
            Title = title;
            Description = description;
            StartAt = startAt;
            EndAt = endAt;
        }
    }
}