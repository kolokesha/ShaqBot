using Microsoft.AspNetCore.Mvc;
using ShaqBot.Entities;
using ShaqBot.Handlers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ShaqBot;

[ApiController]
[Route("api/message/update")]
public class TelegramBotController : ControllerBase
{
    private readonly TelegramBotClient _telegramBotClient;
    private readonly List<QuestionAnswerPair> _questions;
    private readonly DataContext _context;
    private readonly CurrentQuestion _currentQuestion;

    public TelegramBotController(TelegramBot telegramBot, DataContext context, CurrentQuestion question)
    {
        _telegramBotClient = telegramBot.GetBot().Result;
        _questions = PollHandler.AddAnswerPair();
        _context = context;
        _currentQuestion = question;
    }

    [HttpPost]
    public async Task<IActionResult> Update([FromBody] Update update)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    await UpdateHandler.HandleMessageUpdate(update.Message, _telegramBotClient, _currentQuestion, _context);

                    break;
                }
                case UpdateType.CallbackQuery:
                {
                    await UpdateHandler.HandleCallbackQueryUpdate(update.CallbackQuery, _telegramBotClient, _currentQuestion, _context, _questions);
                    break;
                    // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                }
                case UpdateType.PollAnswer:
                {
                    await UpdateHandler.HandlePollAnswer(update, _telegramBotClient, _context, _currentQuestion, _questions);
                    break;
                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return Ok();
    }
}