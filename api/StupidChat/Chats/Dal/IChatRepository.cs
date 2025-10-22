using System;
using System.Threading.Tasks;

public interface IChatRepository
{
    Task<Chat> GetByIdAsync(long chatId);
    Task<Chat> Create(string user);
    Task<Chat> AddAsync(Chat chat);
    Task<Message> AddQuestionAsync(long chatId, Message question);
    Task<ReplyMessage> AddAnswerAsync(long chatId, long questionId, ReplyMessage answer);
    Task<ReplyMessage[]> GetAnswerAsync(long chatId, long messageId);
}
