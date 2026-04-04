using Events.Application.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Events.Application.Extensions
{
    internal static class EventValidatationExtension
    {
        internal static void ValidateOrThrow(this IEventDto dto)
        {
            var errorMessages = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(dto.Title))
            {
                errorMessages.Add(nameof(dto.Title), "Title is required");
            }

            if (dto.StartAt == default)
            {
                errorMessages.Add(nameof(dto.StartAt), "Start date is required");
            }

            if (dto.EndAt == default)
            {
                errorMessages.Add(nameof(dto.EndAt), "End date is required");
            }

            if (dto.EndAt <= dto.StartAt)
            {
                errorMessages.Add(nameof(dto.EndAt), "End date must be later than start date");
            }

            if (errorMessages.Any())
            {
                throw new ValidationException(
                    string.Join(
                        ", ",
                        errorMessages.Select(
                            x => $"{x.Key}: {x.Value}")));
            }
        }
    }
}