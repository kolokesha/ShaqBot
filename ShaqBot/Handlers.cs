using System.Net.Mime;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ShaqBot;

public class Handlers
{
    public async Task UpdateHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    // эта переменная будет содержать в себе все связанное с сообщениями
                    var message = update.Message;

                    // From - это от кого пришло сообщение (или любой другой Update)
                    var user = message.From;

                    // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                    // Chat - содержит всю информацию о чате
                    var chat = message.Chat;

                    switch (message.Type)
                    {
                        case MessageType.Text:
                        {
                            if (message.Text == "/start")
                            {
                                var inlineKeyboard = CreateKeyboard(botClient, chat); // Тут создаем нашу клавиатуру
                                SendMessage(botClient, chat, inlineKeyboard, null); // Все клавиатуры передаются в параметр replyMarkup

                                return;
                            }

                            return;
                        }
                        default:
                        {
                            await botClient.SendTextMessageAsync(
                                chat.Id, "Используй только текст");
                            return;
                        }
                    }
                }
                case UpdateType.CallbackQuery:
                {
                    // Переменная, которая будет содержать в себе всю информацию о кнопке, которую нажали
                    var callbackQuery = update.CallbackQuery;

                    // Аналогично и с Message мы можем получить информацию о чате, о пользователе и т.д.
                    var user = callbackQuery.From;

                    // Выводим на экран нажатие кнопки
                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");

                    // Вот тут нужно уже быть немножко внимательным и не путаться!
                    // Мы пишем не callbackQuery.Chat , а callbackQuery.Message.Chat , так как
                    // кнопка привязана к сообщению, то мы берем информацию от сообщения.
                    var chat = callbackQuery.Message.Chat;

                    // Добавляем блок switch для проверки кнопок
                    switch (callbackQuery.Data)
                    {
                        // Data - это придуманный нами id кнопки, мы его указывали в параметре
                        // callbackData при создании кнопок. У меня это button1, button2 и button3

                        case "button1":
                        {
                            // В этом типе клавиатуры обязательно нужно использовать следующий метод
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id);
                            // Для того, чтобы отправить телеграмму запрос, что мы нажали на кнопку

                            var Questions = AddAnswerPair();
                            var CounterOfDepression = new Result();
                            foreach (var question in Questions)
                            {
                                var pollMessage = await botClient.SendPollAsync(
                                    chat.Id,
                                    question.Question,
                                    question.Answers,
                                    isAnonymous: false,
                                    cancellationToken: cancellationToken);

                                var updates = await botClient.GetUpdatesAsync(0, cancellationToken: cancellationToken);

                                await WaitForPollResultsAsync(botClient, pollMessage, question.Id, cancellationToken,
                                    CounterOfDepression);
                            }

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Количество депрессии: {CounterOfDepression.DepressionCount}");
                            var inlineKeyboard = CreateKeyboard(botClient, chat);
                            SendMessage(botClient, chat, inlineKeyboard, null);
                            return;
                        }

                        case "button2":
                        {
                            // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                            Message message = await botClient.SendDocumentAsync(
                                chatId: chat.Id,
                                document: InputFile.FromUri("https://github.com/kolokesha/ShaqBot/blob/master/%D0%B3%D0%B0%D0%B8%CC%86%D0%B4_%D0%93%D0%A2%D0%A0.pdf"),
                                caption: "<b>Друзья, я вам обещал в этом  <a href=\"https://youtu.be/k0Ic__VZAyo?si=v4VF-b2RujRj3SlF\">ролике</a> дать подробный гайд о том, как можно самостоятельно справиться с ГТР (генерализованное тревожное расстройство). \n\nПрикрепляю его и надеюсь, что это будет первым шагом к здоровому состоянию.</b>.",
                                parseMode: ParseMode.Html,
                                cancellationToken: cancellationToken);

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Вы нажали на {callbackQuery.Data}");
                            var inlineKeyboard = CreateKeyboard(botClient, chat);
                            SendMessage(botClient, chat, inlineKeyboard, null);
                            return;
                        }

                        case "button3":
                        {
                            // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "А это полноэкранный текст!",
                                true);

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                $"Вы нажали на {callbackQuery.Data}");
                            var inlineKeyboard = CreateKeyboard(botClient, chat);
                            SendMessage(botClient, chat, inlineKeyboard, null);
                            return;
                        }
                    }
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private async Task WaitForPollResultsAsync(ITelegramBotClient botClient, Message poll, int? questionId,
        CancellationToken cancellationToken, Result CounterOfDepression)
    {
        while (poll.Poll.TotalVoterCount == 0)
        {
            var updates = await botClient.GetUpdatesAsync(0, cancellationToken: cancellationToken);

            var pollAnswerUpdate = updates.FirstOrDefault(update => update.PollAnswer != null && update.PollAnswer.PollId == poll.Poll.Id);

            if (pollAnswerUpdate != null)
            {
                await botClient.StopPollAsync(
                    poll.Chat.Id,
                    poll.MessageId,
                    cancellationToken: cancellationToken);
                
                await Task.Delay(1000);
                var updatesans = await botClient.GetUpdatesAsync(0, cancellationToken: cancellationToken);
                
                var answerPoll = updatesans.FirstOrDefault(update => update.Poll != null && update.Poll.Id == poll.Poll.Id);
                var answers = answerPoll.Poll.Options; 
                for (var i = 0; i < answers.Length; i++)
                {
                    if (answers[i].VoterCount != 0)
                    {
                        CounterOfDepression.CountDepression(i);
                        break;
                    }
                }
                break;
            }
            await Task.Delay(1000);
        }
    }

    public async void SendMessage(ITelegramBotClient botClient, Chat? chat, InlineKeyboardMarkup? inlineKeyboard, string? text)
    {
        await botClient.SendTextMessageAsync(
            chat.Id,
            "Всем приветики это ТИМУР ШАКИРОВ БОТ",
            replyMarkup: inlineKeyboard);
    }
    private InlineKeyboardMarkup CreateKeyboard(ITelegramBotClient botClient, Chat? chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new
                List<InlineKeyboardButton
                    []>() // здесь создаем лист (массив), который содрежит в себе массив из класса кнопок
                {
                    // Каждый новый массив - это дополнительные строки,
                    // а каждая дополнительная строка (кнопка) в массиве - это добавление ряда

                    new[] // тут создаем массив кнопок
                    {
                        InlineKeyboardButton.WithUrl("Ссылка на чат",
                            "https://t.me/shakirov_psy"),
                        InlineKeyboardButton.WithCallbackData("Тестирование депрессии по Беку", "button1")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Гайд ГТР", "button2"),
                        InlineKeyboardButton.WithCallbackData("И здесь", "button3")
                    }
                });
        
        return inlineKeyboard;
    }
    private List<QuestionAnswerPair> AddAnswerPair()
    {
        var questionAnswerList = new List<QuestionAnswerPair>
        {
            new("1", "Вы чувствуете себя расстроенным?",
                new List<string>
                {
                    "Я не чувствую себя расстроенным, печальным", "Я расстроен",
                    "Я все время расстроен и не могу от этого отключиться",
                    "Я настолько расстроен и несчастлив, что не могу это выдержать"
                }),
            new("2", "Вы тревожитесь о своем будущем?",
                new List<string>
                {
                    "Я не тревожусь о своем будущем", "Я чувствую, что озадачен будущим",
                    "Я чувствую, что меня ничего не ждет в будущем",
                    "Мое будущее безнадежно, и ничто не может измениться к лучшему"
                }),
            new("3", "Вас преследуют неудачи?",
                new List<string>
                {
                    "Я не чувствую себя неудачником",
                    "Я чувствую, что терпел больше неудач, чем другие люди",
                    "Когда я оглядываюсь на свою жизнь, я вижу в ней много неудач",
                    "Я чувствую, что как личность, я – полный неудачник"
                }),
            new("4", "Вы получаете удовлетворение от жизни?",
                new List<string>
                {
                    "Я получаю столько же удовлетворения от жизни, как раньше",
                    "Я не получаю столько же удовлетворения от жизни, как раньше",
                    "Я больше не получаю удовлетворения ни от чего",
                    "Я полностью не удовлетворен жизнью и мне все надоело"
                })
        };
        return questionAnswerList;
    }
}