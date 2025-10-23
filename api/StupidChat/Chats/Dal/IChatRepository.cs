using System;
using System.Threading.Tasks;

public interface IChatRepository
{
    /// <summary>
    /// получение полного чата по id
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    Task<Chat> GetByIdAsync(long chatId);
    
    /// <summary>
    /// создание нового чата
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<Chat> Create(string user);
    
    /// <summary>
    /// создание чата с сообщениями
    /// </summary>
    /// <param name="chat"></param>
    /// <returns></returns>
    Task<Chat> AddAsync(Chat chat);

    /// <summary>
    /// добавление вопроса
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="question"></param>
    /// <returns></returns>
    Task<Message> AddQuestionAsync(long chatId, Message question);

    /// <summary>
    /// добавление ответа на вопрос
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="questionId"></param>
    /// <param name="answer"></param>
    /// <returns></returns>
    Task<ReplyMessage> AddAnswerAsync(long chatId, long questionId, ReplyMessage answer);

    /// <summary>
    /// получение ответов на вопрос
    /// </summary>
    /// <param name="chatId"></param>
    /// <param name="messageId"></param>
    /// <returns></returns>
    Task<ReplyMessage[]> GetAnswerAsync(long chatId, long messageId);
}
