using System;
using System.Text.Json;
using System.Threading.Tasks;
using CourseCatalog.Api.Data;
using CourseCatalog.Api.Models;
using Microsoft.Extensions.Logging;

namespace CourseCatalog.Api.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly CourseDbContext _dbContext;
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(CourseDbContext dbContext, ILogger<EventPublisher> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task PublishAsync<T>(string eventName, T eventData)
        {
            var payloadJson = JsonSerializer.Serialize(eventData);

            // 1. Create OutboxEvent record
            var outboxEvent = new OutboxEvent
            {
                EventType = eventName,
                Payload = payloadJson,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.OutboxEvents.AddAsync(outboxEvent);
            await _dbContext.SaveChangesAsync();

            // 2. Log to console for testing/audit
            _logger.LogInformation("PUBLISHED SYSTEM EVENT [{EventName}] (Saved in Outbox Event Id: {EventId}) @ {Time}. Payload: {Payload}", 
                eventName, outboxEvent.Id, outboxEvent.CreatedAt, payloadJson);
        }
    }
}
