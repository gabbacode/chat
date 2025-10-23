using System.Collections.Generic;

public record struct Chat()
{
    public long Id { get; set; }
    public string User { get; set; }

    // сохраняем порядок сообщений, и более менее быстрый произвольный доступ
    // todo Dictionary + List
    public SortedDictionary<long, Message> Messages { get; init; } = new();
}
