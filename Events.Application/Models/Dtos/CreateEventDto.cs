using Events.Application.Models.Interfaces;

namespace Events.Application.Models.Dtos
{
    public record CreateEventDto : IEventDto
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime StartAt { get; init; }
        public DateTime EndAt { get; init; }
    }
}