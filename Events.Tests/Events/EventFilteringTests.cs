using Events.Application.Models.Dtos;
using Events.Application.Services;
using Events.Domain.Repositories;
using Moq;

namespace Events.Tests.Events
{
    public class EventFilteringTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly EventService _eventService;

        public EventFilteringTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _eventService = new EventService(_mockRepository.Object);
        }

        [Fact]
        public void GetEventsWithTitleFilter()
        {
            var searchTitle = "Event 3";
            var dto = new GetEventsDto
            {
                Page = 1,
                PageSize = 10,
                Title = searchTitle
            };

            var expectedEvents = EventData.TestEvents
                .Where(x => x.Title.Contains(searchTitle))
                .ToList();

            _mockRepository
                .Setup(x =>
                    x.Get(
                        dto.Page,
                        dto.PageSize,
                        dto.Title,
                        dto.From,
                        dto.To))
                .Returns(expectedEvents);

            _mockRepository
                .Setup(x =>
                    x.GetEventsCount(
                        dto.Title,
                        dto.From,
                        dto.To))
                .Returns(expectedEvents.Count);

            var result = _eventService.GetEvents(dto);

            Assert.Single(result.Items);
            Assert.Equal(searchTitle, result.Items.First().Title);
        }

        [Theory]
        [InlineData(1, 25, 1)]
        [InlineData(1, 35, 2)]
        [InlineData(1, 45, 3)]
        public void GetEventsWithDateRangesTest(
            int fromDays, int toDays, int expectedCount)
        {
            var fromDate = DateTime.Now.AddDays(fromDays);
            var toDate = DateTime.Now.AddDays(toDays);
            var dto = new GetEventsDto
            {
                Page = 1,
                PageSize = 10,
                From = fromDate,
                To = toDate
            };

            var expectedEvents = EventData.TestEvents
                .Where(x =>
                    x.StartAt >= fromDate
                    && x.EndAt <= toDate)
                .ToList();

            _mockRepository
                .Setup(x =>
                    x.Get(
                        dto.Page,
                        dto.PageSize,
                        dto.Title,
                        dto.From,
                        dto.To))
                .Returns(expectedEvents);

            _mockRepository
                .Setup(x =>
                    x.GetEventsCount())
                .Returns(expectedEvents.Count);

            var result = _eventService.GetEvents(dto);

            Assert.Equal(expectedCount, result.Items.Count);
        }
    }
}