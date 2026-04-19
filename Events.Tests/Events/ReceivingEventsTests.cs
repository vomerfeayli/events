using Events.Application.Models.Dtos;
using Events.Application.Services;
using Events.Domain.Repositories;
using Moq;

namespace Events.Tests.Events
{
    public class ReceivingEventsTests
    {
        private readonly Mock<IEventRepository> _repositoryMock;
        private readonly EventService _eventService;

        public ReceivingEventsTests()
        {
            _repositoryMock = new Mock<IEventRepository>();
            _eventService = new EventService(_repositoryMock.Object);
        }

        [Fact]
        public void GetEventsTest()
        {
            var dto = new GetEventsDto
            {
                Page = 1,
                PageSize = 10
            };

            _repositoryMock
                .Setup(x =>
                    x.Get(
                        dto.Page,
                        dto.PageSize,
                        dto.Title,
                        dto.From,
                        dto.To))
                .Returns(EventData.TestEvents);

            _repositoryMock
                 .Setup(x =>
                    x.GetEventsCount())
                 .Returns(EventData.TestEvents.Count);

            var result = _eventService.GetEvents(dto);

            Assert.NotNull(result);
            Assert.Equal(5, result.TotalItems);
        }

        [Fact]
        public void GetEventByIdTest()
        {
            var @event = EventData.TestEvents[0];

            _repositoryMock
                .Setup(x =>
                    x.Get(@event.Id))
                .Returns(EventData.TestEvents.First(x => x.Id == @event.Id));

            var result = _eventService.GetEvent(@event.Id);

            Assert.NotNull(result);
            Assert.Equal(@event.Id, result.Id);
            Assert.Equal(@event.Title, result.Title);
            Assert.Equal(@event.Description, result.Description);
            Assert.Equal(@event.StartAt, result.StartAt);
            Assert.Equal(@event.EndAt, result.EndAt);
        }

        [Theory]
        [InlineData(1, 10, 1, 1, 5, 5)]
        [InlineData(1, 1, 1, 5, 5, 1)]
        [InlineData(2, 1, 2, 5, 5, 1)]
        [InlineData(1, 2, 1, 3, 5, 2)]
        [InlineData(1, 3, 1, 2, 5, 3)]
        [InlineData(2, 3, 2, 2, 5, 2)]
        public void GetEventsPagingTest(
            int page,
            int pageSize,
            int expectedCurrentPage,
            int expectedTotalPages,
            int expectedTotalItems,
            int expectedItemsCount)
        {
            var dto = new GetEventsDto
            {
                Page = page,
                PageSize = pageSize
            };

            _repositoryMock
                .Setup(x =>
                    x.Get(page, pageSize, null, null, null))
                .Returns(
                    EventData.TestEvents
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList());

            _repositoryMock
                .Setup(x =>
                    x.GetEventsCount(null, null, null))
                .Returns(EventData.TestEvents.Count);

            var result = _eventService.GetEvents(dto);

            Assert.NotNull(result);
            Assert.Equal(expectedCurrentPage, result.CurrentPage);
            Assert.Equal(expectedTotalPages, result.TotalPages);
            Assert.Equal(expectedTotalItems, result.TotalItems);
            Assert.Equal(expectedItemsCount, result.Items.Count);
        }

        [Theory]
        [InlineData("Event 1", null, null, 1, 1, 1, 1, 1)]
        [InlineData("Event 6", null, null, 1, 1, 0, 0, 0)]
        [InlineData(null, "2024-01-01", null, 1, 10, 5, 5, 5)]
        [InlineData(null, null, "2027-01-01", 1, 10, 5, 5, 5)]
        public void GetEventsWithCombinedFilters(
            string title,
            string fromDate,
            string toDate,
            int page,
            int pageSize,
            int expectedTotalItems,
            int expectedItemsCount,
            int eventsCount)
        {
            DateTime? from = string.IsNullOrEmpty(fromDate)
                ? null
                : DateTime.Parse(fromDate);

            DateTime? to = string.IsNullOrEmpty(toDate)
                ? null
                : DateTime.Parse(toDate);

            var dto = new GetEventsDto
            {
                Page = page,
                PageSize = pageSize,
                Title = title,
                From = from,
                To = to
            };

            var expectedEvents = EventData.TestEvents
                .Where(x =>
                {
                    var filterResult = true;

                    if (!string.IsNullOrEmpty(dto.Title))
                    {
                        filterResult = filterResult && x.Title == dto.Title;
                    }

                    if (dto.From != null)
                    {
                        filterResult = filterResult && x.StartAt >= dto.From;
                    }

                    if (dto.To != null)
                    {
                        filterResult = filterResult && x.EndAt <= dto.To;
                    }

                    return filterResult;
                })
                .ToList();

            _repositoryMock
                .Setup(x =>
                    x.Get(dto.Page, dto.PageSize, dto.Title, dto.From, dto.To))
                .Returns(
                    expectedEvents
                        .Skip((dto.Page - 1) * dto.PageSize)
                        .Take(dto.PageSize)
                        .ToList());

            _repositoryMock
                .Setup(x =>
                    x.GetEventsCount(dto.Title, dto.From, dto.To))
                .Returns(eventsCount);

            var result = _eventService.GetEvents(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Page, result.CurrentPage);
            Assert.Equal(expectedTotalItems, result.TotalItems);
            Assert.Equal(expectedItemsCount, result.Items.Count);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("11111111-1111-1111-1111-111111111111")]
        public void GetEventByIdFailedTest(string guidString)
        {
            var id = Guid.Parse(guidString);

            _repositoryMock
                .Setup(x =>
                    x.Get(id))
                .Throws<KeyNotFoundException>();

            var exception = Assert.Throws<KeyNotFoundException>(
                () => _eventService.GetEvent(id));

            Assert.NotNull(exception);
            Assert.IsType<KeyNotFoundException>(exception);
        }
    }
}