using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// жертвуем удобством, для того чотбы сократить время на треть, так как эффективность это цель из задания

/*
[ApiController]
[Route("api/[controller]")]
public class ChatsController(IChatRepository repository) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<Chat> Get(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    [HttpPost("create")]
    public Task<Chat> Create([FromBody] CreateChatRequest createChatRequest)
    {
        return repository.Create(createChatRequest.User);
    }

    [HttpPost("import")]
    public Task<Chat> Import([FromBody] Chat chat)
    {
        return repository.AddAsync(chat);
    }

    [HttpPost("{id}/ask")]
    public async Task<AskQuestionResponse> AskQuestion(long id, [FromBody] AskQuestionRequest question)
    {
        var newMessage = await repository.AddQuestionAsync(
            id, 
            new Message
            {
                Author = question.Author,
                Text = question.Text
            });

        //if (chat == null)
        //{
        //    return NotFound("Chat not found");
        //}

        return new AskQuestionResponse
        {
            ChatId = id,
            Message = newMessage
        };
    }

    [HttpPost("{id}/answer")]
    public async Task<AnswerQuestionResponse> AnswerQuestion(long id, [FromBody] AnswerQuestionRequest answerQuestionRequest)
    {
        var answer = await repository.AddAnswerAsync(
            id,
            answerQuestionRequest.QuestionId,
            new ReplyMessage
            {
                Author = answerQuestionRequest.Author,
                Text = answerQuestionRequest.Text
            });

        return new AnswerQuestionResponse
        {
            ChatId = id,
            QuestionId = answerQuestionRequest.QuestionId,
            Answer = answer
        };
    }

    [HttpGet("{id}/answer/{questionId}")]
    public async Task<LoadAnswersResponse> LoadAnswers(long id, long questionId)
    {
        var answers = await repository.GetAnswerAsync(id, questionId);

        return new LoadAnswersResponse
        {
            ChatId = id,
            QuestionId = questionId,
            Answers = answers
        };
    }
}
*/