using Events.Application.Models.Dtos;
using Events.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Events.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _events;

        public EventsController(
            IEventService events)
        {
            _events = events;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_events.GetEvents());
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(
            [FromRoute] Guid id)
        {
            return Ok(_events.GetEvent(id));
        }

        [HttpPost]
        public IActionResult Create(
            [FromBody] CreateEventDto eventDto)
        {
            var eventId = _events.CreateEvent(eventDto);

            return Created(string.Empty, eventId);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Update(
            [FromRoute] Guid id,
            [FromBody] UpdateEventDto @event)
        {
            _events.UpdateEvent(id, @event);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(
            [FromRoute] Guid id)
        {
            _events.DeleteEvent(id);
            return NoContent();
        }
    }
}