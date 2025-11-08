namespace WebDevMasterClass.Services.Orders.Entities;

internal class Event
{
    public int Id { get; set; }
    public EventType Type { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Data { get; set; } = string.Empty;
    public EventState State { get; set; }
}

public enum EventType
{
    OrderCreated
}

public enum EventState
{
    Pending,
    Handled
}