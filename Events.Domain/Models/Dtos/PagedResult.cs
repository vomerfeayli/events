namespace Events.Domain.Models.Dtos
{
    public record PagedResult<T>(
        IReadOnlyCollection<T> Items,
        int CurrentPage,
        int TotalPages,
        int TotalItems);
}