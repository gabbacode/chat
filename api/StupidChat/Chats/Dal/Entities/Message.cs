using System;
using System.Collections.Generic;

public record struct Message()
{
    public long Id { get; set; }

    public DateTime Timestamp { get; set; }

    public string Author { get; set; }

    public string Text { get; set; }

    public List<ReplyMessage> Replies { get; private set; } = new List<ReplyMessage>(0);
}
