using Events.Domain.Entities;

namespace Events.Tests.Events
{
    internal static class EventData
    {
        public static readonly List<Event> TestEvents =
        [
            new("Event 1", "Description 1", DateTime.Now.AddDays(10), DateTime.Now.AddDays(20)),
            new("Event 2", "Description 2", DateTime.Now.AddDays(20), DateTime.Now.AddDays(30)),
            new("Event 3", "Description 3", DateTime.Now.AddDays(30), DateTime.Now.AddDays(40)),
            new("Event 4", "Description 4", DateTime.Now.AddDays(40), DateTime.Now.AddDays(50)),
            new("Event 5", "Description 5", DateTime.Now.AddDays(50), DateTime.Now.AddDays(60)),
        ];
    }
}