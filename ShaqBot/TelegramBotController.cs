using Microsoft.AspNetCore.Mvc;
using ShaqBot.Entities;
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
    private readonly Dictionary<string, long> pollChatMap = new();
    private readonly DataContext _context;
    private readonly CurrentQuestion _currentQuestion;

    public TelegramBotController(TelegramBot telegramBot, DataContext context, CurrentQuestion question)
    {
        _telegramBotClient = telegramBot.GetBot().Result;
        _questions = AddAnswerPair();
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
                    var message = update.Message;
                    
                    var user = message.From;

                    
                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                    // Chat - содержит всю информацию о чате
                    var chat = message.Chat;

                    switch (message.Type)
                    {
                        case MessageType.Text:
                        {
                            if (message.Text == "/start")
                            {
                                var inlineKeyboard = CreateKeyboard(_telegramBotClient, chat);
                                var thumbMessage = await _telegramBotClient.SendPhotoAsync(
                                    chat.Id,
                                    InputFile.FromUri("https://imgur.com/a/MfHE7rC")
                                );

                                await _telegramBotClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Я пcихoлог с научно-доказательным подходом.\n\nТак вышло, что я лично пpошёл чеpез выгopание, апaтию и oбеcценивание, на себе прочувствовал страх совершить ошибку. Я знаю, что такое прокрастинация и перфекционизм. Мне хорошо знакома вечная тревога то за себя, то за других.\n\nОднако я справился с каждой из этих проблем и теперь знаю, как работать без выгорания. И жить без тревоги, страха и паники. \n\nЯ веду частные консультации с 2019 года, а еще за моими плечами:\n\n\ud83d\udcda Школа эмоционального интеллекта и психотерапии\n\ud83d\udcda членство в Ассоциации когнитивно-поведенческой терапии\n\ud83d\udcda благотворительная помощь в Красном Кресте\n\nБолее 120 человек уже смогли избавиться от страха и паники с моей помощью. \n\nЕсли вы тоже хотите уменьшить тревожность, научиться управлять своими мыслями и эмоциями — я готов помочь.  \n\nБыстрых изменений не обещаю, но вместе мы можем в тщательной работе с тревожностью, страхами и убеждениями прийти к здоровому эмоциональному состоянию, чтобы вы смогли развиваться быстрее и стабильнее.\n\n\ud83d\udc8c Напишите мне личное сообщение (@ShakirovTimur),\nесли хотите:\n\n\ud83d\udd11 побороть перфекционизм и страх ошибок;\n\ud83d\udd11 преодолеть страх продвигаться и проявляться в соц. сетях;\n\ud83d\udd11 убрать высокие требования к себе и прокрастинацию;\n\ud83d\udd11 научиться грамотно распределять работу и отдых;\n\ud83d\udd11 избавиться от тревожности и панических атак;\n\ud83d\udd11 не бояться принимать решения и нести за них ответственность;\n\ud83d\udd11 справиться с выгоранием или депрессией;\n\ud83d\udd11 научиться жить спокойно и уверенно.\n\n\ud83d\udcddПолучив ваше сообщение, я отправлю вам небольшую анкету, и мы договоримся о времени первой встречи.\nПосле каждой консультации вы будете получать задания на дом, а задавать вопросы сможете и вне сессий — я отвечу на них в течение дня.\n\n⁉\ufe0f Часто, если вопрос не требует долгой работы, я отвечаю голосовым сообщением совершенно бесплатно, потому что понимаю, насколько важна поддержка. Кроме того, некоторые вопросы можно решить самостоятельно, не проходя длительную терапию.  \n\nЖду ваших сообщений! ",
                                    replyMarkup: inlineKeyboard); // Тут создаем нашу клавиатуру

                                // Все клавиатуры передаются в параметр replyMarkup

                                var replyKeyboard = new ReplyKeyboardMarkup(
                                    new List<KeyboardButton[]>
                                    {
                                        new KeyboardButton[]
                                        {
                                            new("Меню")
                                        }
                                    })
                                {
                                    // автоматическое изменение размера клавиатуры, если не стоит true,
                                    // тогда клавиатура растягивается чуть ли не до луны,
                                    // проверить можете сами
                                    ResizeKeyboard = true
                                };
                                return Ok();
                            }

                            if (message.Text == "Меню")
                            {
                                var inlineKeyboard = CreateKeyboard(_telegramBotClient, chat);
                                var thumbMessage = await _telegramBotClient.SendPhotoAsync(
                                    chat.Id,
                                    InputFile.FromUri("https://imgur.com/a/MfHE7rC")
                                );

                                await _telegramBotClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Я пcихoлог с научно-доказательным подходом.\n\nТак вышло, что я лично пpошёл чеpез выгopание, апaтию и oбеcценивание, на себе прочувствовал страх совершить ошибку. Я знаю, что такое прокрастинация и перфекционизм. Мне хорошо знакома вечная тревога то за себя, то за других.\n\nОднако я справился с каждой из этих проблем и теперь знаю, как работать без выгорания. И жить без тревоги, страха и паники. \n\nЯ веду частные консультации с 2019 года, а еще за моими плечами:\n\n\ud83d\udcda Школа эмоционального интеллекта и психотерапии\n\ud83d\udcda членство в Ассоциации когнитивно-поведенческой терапии\n\ud83d\udcda благотворительная помощь в Красном Кресте\n\nБолее 120 человек уже смогли избавиться от страха и паники с моей помощью. \n\nЕсли вы тоже хотите уменьшить тревожность, научиться управлять своими мыслями и эмоциями — я готов помочь.  \n\nБыстрых изменений не обещаю, но вместе мы можем в тщательной работе с тревожностью, страхами и убеждениями прийти к здоровому эмоциональному состоянию, чтобы вы смогли развиваться быстрее и стабильнее.\n\n\ud83d\udc8c Напишите мне личное сообщение (@ShakirovTimur),\nесли хотите:\n\n\ud83d\udd11 побороть перфекционизм и страх ошибок;\n\ud83d\udd11 преодолеть страх продвигаться и проявляться в соц. сетях;\n\ud83d\udd11 убрать высокие требования к себе и прокрастинацию;\n\ud83d\udd11 научиться грамотно распределять работу и отдых;\n\ud83d\udd11 избавиться от тревожности и панических атак;\n\ud83d\udd11 не бояться принимать решения и нести за них ответственность;\n\ud83d\udd11 справиться с выгоранием или депрессией;\n\ud83d\udd11 научиться жить спокойно и уверенно.\n\n\ud83d\udcddПолучив ваше сообщение, я отправлю вам небольшую анкету, и мы договоримся о времени первой встречи.\nПосле каждой консультации вы будете получать задания на дом, а задавать вопросы сможете и вне сессий — я отвечу на них в течение дня.\n\n⁉\ufe0f Часто, если вопрос не требует долгой работы, я отвечаю голосовым сообщением совершенно бесплатно, потому что понимаю, насколько важна поддержка. Кроме того, некоторые вопросы можно решить самостоятельно, не проходя длительную терапию.  \n\nЖду ваших сообщений! ",
                                    replyMarkup: inlineKeyboard);
                            }

                            return Ok();
                        }
                        default:
                        {
                            await _telegramBotClient.SendTextMessageAsync(
                                chat.Id, "Используй только текст");
                            return Ok();
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
                            await _telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                            _currentQuestion._currentQuestionIndex = -1;

                            var chatId = update.CallbackQuery.Message.Chat.Id;
                            
                            var buttonKeyboard = new InlineKeyboardMarkup(new[]
                            {
                                new[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Начать тестирование", "test_dm")
                                }
                            });
                            await _telegramBotClient.SendTextMessageAsync(
                                chatId,
                                "Подготовьтесь к тесту \ud83e\uddd8\u200d\u2642\ufe0f. Найдите тихое место, отключите отвлечения, почувствуйте свои эмоции. Отвечайте искренне — это ключ к лучшему самопониманию. Удачи! \ud83c\udf1f",
                                replyMarkup: buttonKeyboard);

                            

                            return Ok();
                        }

                        case "button2":
                        {
                            var thumbMessage = await _telegramBotClient.SendPhotoAsync(
                                chat.Id,
                                InputFile.FromUri("https://imgur.com/a/FCZSnm5")
                            );

                            // А здесь мы добавляем наш сообственный текст, который заменит слово "загрузка", когда мы нажмем на кнопку
                            var message = await _telegramBotClient.SendDocumentAsync(
                                chat.Id,
                                InputFile.FromUri(
                                    "https://github.com/kolokesha/ShaqBot/blob/master/ShaqBot/Sources/%D0%B3%D0%B0%D0%B8%CC%86%D0%B4_%D0%93%D0%A2%D0%A0.pdf"),
                                caption:
                                "<b>Друзья, я вам обещал в этом  <a href=\"https://youtu.be/k0Ic__VZAyo?si=v4VF-b2RujRj3SlF\">ролике</a> дать подробный гайд о том, как можно самостоятельно справиться с ГТР (генерализованное тревожное расстройство). \n\nПрикрепляю его и надеюсь, что это будет первым шагом к здоровому состоянию.</b>.",
                                parseMode: ParseMode.Html,
                                thumbnail: InputFile.FromUri(
                                    "https://github.com/kolokesha/ShaqBot/blob/master/ShaqBot/Sources/Preview.png"));
                            return Ok();
                        }

                        case "button3":
                        {
                            // А тут мы добавили еще showAlert, чтобы отобразить пользователю полноценное окно
                            await _telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id,
                                "А это полноэкранный текст!",
                                true);

                            await _telegramBotClient.SendTextMessageAsync(
                                chat.Id,
                                $"Вы нажали на {callbackQuery.Data}");
                            return Ok();
                        }
                        case "test_dm":
                        {
                            await _telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                            _currentQuestion._currentQuestionIndex = -1;

                            var chatId = update.CallbackQuery.Message.Chat.Id;
                            
                            await SendNextPoll(chatId);
                            
                            return Ok();
                        }
                    }

                    return Ok();
                }
                case UpdateType.PollAnswer:
                {
                    var CounterOfDepression = new Result();
                    ProcessPollAnswer(update.PollAnswer, CounterOfDepression);
                    var pollId = update.PollAnswer.PollId;
                    var chatId = GetChatIdForPoll(pollId);
                    await SendNextPoll(chatId);

                    return Ok();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return Ok();
    }

    private long? GetChatIdForPoll(string pollId)
    {
        var mapping = _context.PollChatMaps.AsEnumerable().FirstOrDefault(x => x.PollId == pollId);
        if (mapping != null) return mapping.ChatId;

        // Если опрос не найден, вы можете вернуть значение по умолчанию или обработать случай соответствующим образом
        return 0;
    }

    private void ProcessPollAnswer(PollAnswer pollAnswer, Result CounterOfDepression)
    {
        var answers = pollAnswer.OptionIds;
        foreach (var answer in answers) CounterOfDepression.CountDepression(answer);
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
                        InlineKeyboardButton.WithUrl("Записаться на консультацию", "https://t.me/ShakirovTimur")
                    }
                });

        return inlineKeyboard;
    }

    private async Task SendNextPoll(long? chatId)
    {
        _currentQuestion.Add();


        if (_currentQuestion._currentQuestionIndex < _questions.Count)
        {
            var currentQuestion = _questions[_currentQuestion._currentQuestionIndex];
            if (_telegramBotClient != null)
            {
                var pollMessage = await _telegramBotClient.SendPollAsync(
                    chatId,
                    currentQuestion.Question,
                    currentQuestion.Answers,
                    isAnonymous: false);

                var mapping = new PollChatMap
                {
                    PollId = pollMessage.Poll.Id,
                    ChatId = chatId
                };
                _context.PollChatMaps.Add(mapping);
                _context.SaveChanges();
            }
        }
        else
        {
            // All questions have been sent, end the poll sequence
            _currentQuestion._currentQuestionIndex = -1;
            var result = Result.ReturnResult();
            if (Result.ReturnResult() <= 13)
            {
                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"депрессивных симптомов нет. С вашим психическим здоровьем всё в порядке. Ваш уровень депрессии:" +
                    $" {result} баллов." +
                    $"В случае, если вы хотите избавиться от каких-либо страхов или фобий, записывайтесь на консультацию по ссылке");
            }

            if (Result.ReturnResult() <= 19 && Result.ReturnResult() > 13)
            {
                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"Вероятна лёгкая депрессия (субдепрессия). Ваш уровень депрессии:" +
                    $" {result} баллов." +
                    $"В случае, если вы хотите избавиться от каких-либо страхов или фобий, записывайтесь на консультацию по ссылке");
            }

            if (result <= 28 && result > 20)
            {
                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"Умеренная депрессия. Советуем обратиться к врачу. Ваш уровень депрессии:" +
                    $" {result} баллов." +
                    $"В случае, если вы хотите избавиться от каких-либо страхов или фобий, записывайтесь на консультацию по ссылке");
            }

            if (result <= 63 && result > 29)
            {
                await _telegramBotClient.SendTextMessageAsync(
                    chatId,
                    $"Тяжёлая депрессия. Состояние тем сложнее, чем больше количество баллов. Советуем обратиться к врачу. Ваш уровень депрессии:" +
                    $" {result} баллов." +
                    $"В случае, если вы хотите избавиться от каких-либо страхов или фобий");
            }

            var buttonKeyboard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Записаться на консультацию", "send_dm")
                }
            });
            await _telegramBotClient.SendTextMessageAsync(
                chatId,
                "\ud83c\udf08 Забота о своем психическом здоровье — ключ к гармоничной жизни! \ud83e\udde0\u2728 Если вам кажется, что ваши эмоции стали слишком тяжёлыми, а повседневные стрессовые ситуации начинают влиять на ваше благополучие, не стесняйтесь обратиться за помощью.\n\n\ud83c\udf1f Я готов предоставить вам поддержку и консультации. Работая в тёплой и доверительной обстановке, мы вместе постараемся понять ваши чувства, найти решения и дать вам необходимые инструменты для преодоления трудностей.\n\n\ud83d\udcac Для записи на консультацию, отправьте нам личное сообщение, и мы свяжемся с вами. Важно помнить, что забота о своем внутреннем мире — это забота о себе. Вместе мы сделаем первый шаг к психологическому благополучию. \ud83c\udf37\ud83c\udf3f",
                replyMarkup: buttonKeyboard);
        }
    }

    private List<QuestionAnswerPair> AddAnswerPair()
    {
        return new List<QuestionAnswerPair>
        {
            new("1", "Вы чувствуете себя расстроенным?",
                new List<string>
                {
                    "Я не чувствую себя расстроенным, печальным", "Я расстроен",
                    "Я расстроен",
                    "Я все время расстроен и не могу от этого отключиться",
                    "Я настолько расстроен и несчастлив, что не могу это выдержать"
                }),
            new("2", "Вы тревожитесь о своем будущем?",
                new List<string>
                {
                    "Я не тревожусь о своем будущем", "Я чувствую, что озадачен будущим",
                    "Я чувствую, что озадачен будущим",
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
                }),
            new("5", "Вы чувствуете себя виноватым?",
                new List<string>
                {
                    "Я не чувствую себя в чем-нибудь виноватым",
                    "Достаточно часто я чувствую себя виноватым",
                    "Большую часть времени я чувствую себя виноватым",
                    "Я постоянно испытываю чувство вины"
                }),
            new("6", "Вы чувствуете, что Вас можно за что-либо наказать?",
                new List<string>
                {
                    "Я не чувствую, что могу быть наказанным за что-либо",
                    "Я чувствую, что могу быть наказан",
                    "Я ожидаю, что могу быть наказан",
                    "Я чувствую себя уже наказанным"
                }),
            new("7", "Вы разочарованы в себе?",
                new List<string>
                {
                    "Я не разочарован в себе",
                    "Я разочарован в себе",
                    "Я себе противен",
                    "Я себя ненавижу"
                }),
            new("8", "Вы критично относитесь к себе?",
                new List<string>
                {
                    "Я знаю, что я не хуже других",
                    "Я критикую себя за ошибки и слабости",
                    "Я все время обвиняю себя за свои поступки",
                    "Я виню себя во всем плохом, что происходит"
                }),
            new("9", "Вы думаете о самоубийстве?",
                new List<string>
                {
                    "Я никогда не думал покончить с собой",
                    "Ко мне приходят мысли покончить с собой, но я не буду их осуществлять",
                    "Я хотел бы покончить с собой",
                    "Я бы убил себя, если бы представился случай"
                }),
            new("10", "Вы стали чаще плакать?",
                new List<string>
                {
                    "Я плачу не больше, чем обычно",
                    "Сейчас я плачу чаще, чем раньше",
                    "Теперь я все время плачу",
                    "Раньше я мог плакать, а сейчас не могу, даже если мне хочется"
                }),
            new("11", "Вы замечаете, что стали более раздражительны?",
                new List<string>
                {
                    "Сейчас я раздражителен не более, чем обычно",
                    "Я более легко раздражаюсь, чем раньше",
                    "Теперь я постоянно чувствую, что раздражен",
                    "Я стал равнодушен к вещам, которые меня раньше раздражали"
                }),
            new("12", "Вы интересуетесь другими людьми?",
                new List<string>
                {
                    "Я не утратил интереса к другим людям",
                    "Я меньше интересуюсь другими людьми, чем раньше",
                    "Я почти потерял интерес к другим людям",
                    "Я полностью утратил интерес к другим людям"
                }),
            new("13", "Вам стало сложнее принимать решения?",
                new List<string>
                {
                    "Я откладываю принятие решения иногда, как и раньше",
                    "Я чаще, чем раньше, откладываю принятие решения",
                    "Мне труднее принимать решения, чем раньше",
                    "Я больше не могу принимать решения"
                }),
            new("14", "Что Вы думаете о своей внешности?",
                new List<string>
                {
                    "Я не думаю, что выгляжу хуже, чем обычно",
                    "Меня тревожит, что я выгляжу старым и непривлекательным",
                    "Я знаю, что в моей внешности произошли существенные изменения, делающие меня непривлекательным",
                    "Я знаю, что выгляжу безобразно"
                }),
            new("15", "Вы с легкостью выполняете какую-либо работу?",
                new List<string>
                {
                    "Я могу работать так же хорошо, как и раньше",
                    "Мне приходится прилагать дополнительные усилия, чтобы начать делать что-нибудь",
                    "Я с трудом заставляю себя делать что-либо",
                    "Я совсем не могу выполнять никакую работу"
                }),
            new("16", "Вы хорошо спите?",
                new List<string>
                {
                    "Я сплю так же хорошо, как и раньше",
                    "Сейчас я сплю хуже, чем раньше",
                    "Я просыпаюсь на 1-2 часа раньше, и мне трудно заснуть опять",
                    "Я просыпаюсь на несколько часов раньше обычного и больше не могу заснуть"
                }),
            new("17", "Вы стали чаще уставать?",
                new List<string>
                {
                    "Я устаю не больше, чем обычно",
                    "Теперь я устаю быстрее, чем раньше",
                    "Я устаю почти от всего, что я делаю",
                    "Я не могу ничего делать из-за усталости"
                }),
            new("18", "У Вас изменился аппетит?",
                new List<string>
                {
                    "Мой аппетит не хуже, чем обычно",
                    "Мой аппетит стал хуже, чем раньше",
                    "Мой аппетит теперь значительно хуже",
                    "У меня вообще нет аппетита"
                }),
            new("19", "У Вас изменился вес?",
                new List<string>
                {
                    "В последнее время я не похудел или потеря веса была незначительной",
                    "За последнее время я потерял более 2 кг",
                    "Я потерял более 5 кг",
                    "Я потерял более 7 кr"
                }),
            new("20", "Вы беспокоитесь о своем здоровье?",
                new List<string>
                {
                    "Я беспокоюсь о своем здоровье не больше, чем обычно",
                    "Меня тревожат проблемы моего физического здоровья. Такие как боли, расстройства желудка, запоры...",
                    "Я очень обеспокоен своим физическим состоянием, и мне трудно думать о чем-либо другом",
                    "Я настолько обеспокоен своим физическим состоянием, что больше ни о чем не могу думать"
                }),
            new("21", "У Вас изменилось сексуальное влечение?",
                new List<string>
                {
                    "В последнее время я не замечал изменения своего интереса к сексу",
                    "Меня меньше занимают проблемы секса, чем раньше",
                    "Сейчас я значительно меньше интересуюсь сексуальными проблемами, чем раньше",
                    "Я полностью утратил сексуальный интерес"
                })
        };
    }
}