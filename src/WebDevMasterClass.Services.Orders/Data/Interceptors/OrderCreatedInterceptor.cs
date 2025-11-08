using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebDevMasterClass.Services.Orders.Entities;

namespace WebDevMasterClass.Services.Orders.Data.Interceptors;

public class OrderCreatedInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var events = eventData.Context!.ChangeTracker.Entries<Order>()
            .Where(x => x.State == EntityState.Added)
            .Select(x => new Event
            {
                Type = EventType.OrderCreated,
                State = EventState.Pending,
                Date = DateTimeOffset.UtcNow,
                Data = x.Entity.OrderId
            })
            .ToArray();

        eventData.Context.AddRange(events);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    public static OrderCreatedInterceptor Instance { get; } = new();
}