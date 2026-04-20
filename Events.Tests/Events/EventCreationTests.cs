using Events.Application.Models.Dtos;
using Events.Application.Services;
using Events.Domain.Repositories;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Events.Tests.Events
{
    public class EventCreationTests
    {
        private readonly Mock<IEventRepository> _mockRepository;
        private readonly EventService _eventService;

        public EventCreationTests()
        {
            _mockRepository = new Mock<IEventRepository>();
            _eventService = new EventService(_mockRepository.Object);
        }

        [Fact]
        public void CreateEventTest()
        {
            var dto = new CreateEventDto
            {
                Title = "Title",
                Description = "Description",
                StartAt = DateTime.Now.AddDays(15),
                EndAt = DateTime.Now.AddDays(45)
            };

            var result = _eventService.CreateEvent(dto);

            Assert.NotEqual(Guid.Empty, result);
        }

        [Fact]
        public void CreateEventWithNonValidDataTest()
        {
            var dto = new CreateEventDto
            {
                Title = null,
                Description = "Description",
                StartAt = DateTime.Now.AddDays(15),
                EndAt = DateTime.Now.AddDays(45)
            };

            var exception = Assert.Throws<ValidationException>(() =>
                _eventService.CreateEvent(dto));
        }
    }
}