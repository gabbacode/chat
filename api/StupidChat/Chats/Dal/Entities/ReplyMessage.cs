using System;

public record struct ReplyMessage
{
    public long Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Author { get; set; }

    public string Text { get; set; }
}