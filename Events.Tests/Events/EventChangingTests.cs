using Events.Application.Models.Dtos;
using Events.Application.Services;
using Events.Domain.Repositories;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Events.Tests.Events
{
    public class EventChangingTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly EventService _eventService;

        public EventChangingTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _eventService = new EventService(_mockRepository.Object);
        }

        [Fact]
        public void UpdateEventTest()
        {
            var eventForUpdate = EventData.TestEvents[0];

            var dto = new UpdateEventDto
            {
                Title = "Changed title",
                Description = "Changed description",
                StartAt = eventForUpdate.StartAt.AddDays(5),
                EndAt = eventForUpdate.EndAt.AddDays(5)
            };

            _mockRepository
                .Setup(x =>
                    x.Get(eventForUpdate.Id))
                .Returns(eventForUpdate);

            _eventService.UpdateEvent(eventForUpdate.Id, dto);

            _mockRepository.Verify(x =>
                x.Update(
                    It.IsAny<Guid>(),
                    It.Is<string>(title => title == dto.Title),
                    It.Is<string>(description => description == dto.Description),
                    It.Is<DateTime>(startAt => startAt == dto.StartAt),
                    It.Is<DateTime>(endAt => endAt == dto.EndAt)),
                Times.Once);
        }

        [Fact]
        public void DeleteEventTest()
        {
            var eventIdForDelete = EventData.TestEvents[0].Id;

            _mockRepository
                .Setup(x =>
                    x.Delete(eventIdForDelete))
                .Verifiable();

            _eventService.DeleteEvent(eventIdForDelete);

            _mockRepository.Verify(x => x.Delete(eventIdForDelete), Times.Once);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000")]
        [InlineData("11111111-1111-1111-1111-111111111111")]
        public void UpdateEventWithNonExistsIdTest(string guidString)
        {
            var id = Guid.Parse(guidString);

            var dto = new UpdateEventDto
            {
                Title = "Changed title",
                Description = "Changed description",
                StartAt = DateTime.Now.AddDays(5),
                EndAt = DateTime.Now.AddDays(25)
            };

            _mockRepository
                .Setup(x =>
                    x.Update(id, dto.Title, dto.Description, dto.StartAt, dto.EndAt))
                .Throws<KeyNotFoundException>();

            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _eventService.UpdateEvent(id, dto));

            _mockRepository.Verify(x => x.Update(id, dto.Title, dto.Description, dto.StartAt, dto.EndAt), Times.Once);
        }

        [Fact]
        public void CreateUpdateWithNonValidDatesTest()
        {
            var dto = new UpdateEventDto
            {
                Title = "Changed title",
                Description = "Changed description",
                StartAt = DateTime.Now,
                EndAt = DateTime.Now.AddDays(-5)
            };

            var exception = Assert.Throws<ValidationException>(() =>
                _eventService.UpdateEvent(Guid.Empty, dto));
        }
    }
}