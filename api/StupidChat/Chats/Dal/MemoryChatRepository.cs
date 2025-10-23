using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class MemoryChatRepository : IChatRepository
{
    // заменен список на потокобезопасный словарь с быстрым поисокм по ключу
    private readonly ConcurrentDictionary<long, Chat> chats = new();

    // считаем что активность в чате невысокая и нет нужны хранить блокировку для каждого чата
    private readonly ReaderWriterLockSlim[] lockStripes = new ReaderWriterLockSlim[1024];

    public MemoryChatRepository()
    {
        for (int i = 0; i < lockStripes.Length; i++)
            lockStripes[i] = new ReaderWriterLockSlim();
    }

    // PK для чатов
    private long chatId = 0;

    // PK для сообщений, сквозной по всем чатам, вдруг цитирование, пересылка и переходы понадобится
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

        var locker = GetLock(chatId);
        locker.EnterWriteLock();
        try
        {
            question.Id = messageId++;
            question.Timestamp = DateTime.UtcNow;

            chat.Messages.Add(question.Id, question);
        }
        finally 
        { 
            locker.ExitWriteLock();
        }

        return Task.FromResult(question);
    }

    public Task<ReplyMessage[]> GetAnswerAsync(long chatId, long messageId)
    {
        chats.TryGetValue(chatId, out var chat);

        var locker = GetLock(chatId);
        locker.EnterReadLock();
        try
        {
            var message = chat.Messages[messageId];
            return Task.FromResult(
                message.Replies.ToArray());
        }
        finally
        {
            locker.ExitReadLock();
        }
    }

    public Task<ReplyMessage> AddAnswerAsync(long chatId, long questionId, ReplyMessage answer)
    {
        chats.TryGetValue(chatId, out var chat);

        var locker = GetLock(chatId);
        locker.EnterWriteLock();
        try
        {
            answer.Id = messageId++;
            answer.Timestamp = DateTime.UtcNow;
            chat.Messages[questionId].Replies.Add(answer);
        }
        finally 
        {
            locker.ExitWriteLock();
        }

        return Task.FromResult(answer);
    }

    private ReaderWriterLockSlim GetLock(long chatId)
    {
        int stripe = Math.Abs(chatId.GetHashCode()) % lockStripes.Length;
        return lockStripes[stripe];
    }
}