namespace Events.Application.Models.Interfaces
{
    internal interface IEventDto
    {
        string Title { get; init; }
        string Description { get; init; }
        DateTime StartAt { get; init; }
        DateTime EndAt { get; init; }
    }
}