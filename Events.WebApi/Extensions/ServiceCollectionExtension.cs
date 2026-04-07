using Events.Application.Services;
using Events.Application.Services.Interfaces;
using Events.Domain.Repositories;
using Events.Persistence.Repositories;

namespace Events.WebApi.Extensions
{
    internal static class ServiceCollectionExtension
    {
        internal static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSwaggerGen();

            services.AddScoped<IEventService, EventService>();

            services.AddSingleton<IEventRepository, EventsInMemoryRepository>();

            return services;
        }
    }
}