namespace Events.Application.Models.Dtos
{
    public class GetEventsDto
    {
        public string Title { get; init; }
        public DateTime? From { get; init; }
        public DateTime? To { get; init; }
    }
}