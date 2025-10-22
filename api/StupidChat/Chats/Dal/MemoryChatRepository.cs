using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class MemoryChatRepository : IChatRepository
{
    private readonly ConcurrentDictionary<long, Chat> chats = new();
    private readonly object[] lockStripes = new object[1024];

    public MemoryChatRepository()
    {
        for (int i = 0; i < lockStripes.Length; i++)
            lockStripes[i] = new object();
    }

    private long chatId = 0;
    private long messageId = 0;

    public Task<Chat> GetByIdAsync(long chatId)
    {
        chats.TryGetValue(chatId, out Chat chat);

        return Task.FromResult(chat);
    }

    public Task<Chat> AddAsync(Chat chat)
    {
        chat.Id = Interlocked.Increment(ref chatId);

        if (!chats.TryAdd(chat.Id, chat))
            throw new Exception("primary key duplication");
        
        return Task.FromResult(chat);
    }

    public Task<Chat> Create(string user)
    {
        var newChat = new Chat
        {
            User = user
        };

        return AddAsync(newChat);
    }

    public Task<Message> AddQuestionAsync(long chatId, Message question)
    {
        chats.TryGetValue(chatId, out var chat);

        lock (GetLock(chatId))
        {
            question.Id = messageId++;
            question.Timestamp = DateTime.UtcNow;

            chat.Messages.Add(question.Id, question);
        }

        return Task.FromResult(question);
    }

    public Task<ReplyMessage[]> GetAnswerAsync(long chatId, long messageId)
    {
        chats.TryGetValue(chatId, out var chat);

        lock (GetLock(chatId))
        {
            var message = chat.Messages[messageId];
            return Task.FromResult(
                message.Replies.ToArray());   
        }
    }

    public Task<ReplyMessage> AddAnswerAsync(long chatId, long questionId, ReplyMessage answer)
    {
        chats.TryGetValue(chatId, out var chat);

        lock (GetLock(chatId))
        {
            answer.Id = messageId++;
            answer.Timestamp = DateTime.UtcNow;
            chat.Messages[questionId].Replies.Add(answer);
        }

        return Task.FromResult(answer);
    }

    private object GetLock(long chatId)
    {
        int stripe = Math.Abs(chatId.GetHashCode()) % lockStripes.Length;
        return lockStripes[stripe];
    }
}