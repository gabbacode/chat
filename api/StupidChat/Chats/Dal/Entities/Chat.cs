using System.Collections.Generic;

public record struct Chat()
{
    public long Id { get; set; }
    public string User { get; set; }
    public SortedDictionary<long, Message> Messages { get; init; } = new();
}
