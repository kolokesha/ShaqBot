using ShaqBot.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ShaqBot.Handlers;

public class UpdateHandler
{
    public static async Task HandleMessageUpdate(Message message, ITelegramBotClient telegramBotClient,
        CurrentQuestion _currentQuestion, DataContext context)
    {
        if (message == null || message.Type != MessageType.Text)
            return;

        var chat = message.Chat;
        var user = message.From;

        var mainKeyboard = KeyboardHandler.CreateMainKeyboard(telegramBotClient, chat);

        var mainText =
            $"Привет, {user.FirstName} \ud83d\udc4b\ud83c\udffc Ты попал в Телеграм-бота психолога Тимура Шакирова. \ud83e\udd16 \n\nЕсли ты стремишься снизить тревожность, победить фобии и научиться управлять своими мыслями, я готов тебе помочь. \ud83d\udcac\ud83e\udde0";

        if (message.Text is "/start" or "/menu")
            await SendMessageHandler.SendMessage(telegramBotClient, chat.Id, mainText, mainKeyboard, "menu.jpg", context);
        else
            await telegramBotClient.SendTextMessageAsync(
                chat.Id, "Используй команды в боковом меню");
    }

    public static async Task HandleCallbackQueryUpdate(CallbackQuery callbackQuery,
        ITelegramBotClient telegramBotClient, CurrentQuestion _currentQuestion, DataContext context,
        List<QuestionAnswerPair> _questions)
    {
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
                await telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                _currentQuestion._currentQuestionIndex = -1;

                var ListTestKeyboard = KeyboardHandler.ListTestKeyboard(telegramBotClient, chat);

                var message =
                    "Добро пожаловать! \ud83d\ude0a Ниже представлен список тестов по психологии, которые вы можете пройти для более глубокого понимания себя.\n\nКаждый тест предлагает уникальный взгляд на вашу личность, эмоциональный мир и мыслительные процессы.\n\nПройдите тесты с открытым умом и готовностью к самопознанию. Ваша искренность и открытость помогут вам получить максимальную пользу от этого опыта.\n\nПриступим! \ud83d\ude80";

                await SendMessageHandler.SendMessage(telegramBotClient, chat.Id, message, ListTestKeyboard, "questions.jpg", context);

                break;
            }

            case "button2":
            {
                var backMarkup = KeyboardHandler.BackKeyboard(telegramBotClient, chat);
                
                var thumbMessage = await telegramBotClient.SendPhotoAsync(
                    chat.Id,
                    InputFile.FromUri("https://imgur.com/a/FCZSnm5")
                );
                
                var message = await telegramBotClient.SendDocumentAsync(
                    chat.Id,
                    InputFile.FromUri(
                        "https://github.com/kolokesha/ShaqBot/blob/master/ShaqBot/Sources/%D0%B3%D0%B0%D0%B8%CC%86%D0%B4_%D0%93%D0%A2%D0%A0.pdf"),
                    caption:
                    "<b>Друзья, я вам обещал в этом  <a href=\"https://youtu.be/k0Ic__VZAyo?si=v4VF-b2RujRj3SlF\">ролике</a> дать подробный гайд о том, как можно самостоятельно справиться с ГТР (генерализованное тревожное расстройство). \n\nПрикрепляю его и надеюсь, что это будет первым шагом к здоровому состоянию.</b>.",
                    parseMode: ParseMode.Html,
                    replyMarkup: backMarkup,
                    thumbnail: InputFile.FromUri(
                        "https://github.com/kolokesha/ShaqBot/blob/master/ShaqBot/Sources/Preview.png"));
                break;
            }
            case "test_beck":
            {
                var StartTestKeyboard = KeyboardHandler.StartTestKeyboard(telegramBotClient, chat);

                var message =
                    "В тесте 22 вопроса: вам предлагается ряд утверждений, где нужно выбрать только одно, которое лучше всего описывает ваше состояние за прошедшую неделю, включая сегодняшний день. \n\n \u2757\ufe0f Онлайн тест не может быть использован для самостоятельной постановки диагноза! В случае любых сомнений обращайтесь к квалифицированным специалистам. ";

                await SendMessageHandler.SendMessage(telegramBotClient, chat.Id, message, StartTestKeyboard,
                    "instructions.jpg", context);

                break;
            }
            case "free-attach":
            {
                var FreeAttachKeyboard = KeyboardHandler.FreeAttachKeyboard(telegramBotClient, chat);

                var message =
                    "Предоставляем вам доступ к ряду бесплатных материалов, которые могут быть полезны для вашего развития и обогащения знаний. \ud83d\ude0a В этом наборе вы найдете:\n\n- Книги \ud83d\udcda,\n- Гайды \ud83c\udf93,\n- Видеоуроки \ud83c\udfa5,\n- Кейсы \ud83d\udcbb.\n\nЭти материалы покрывают широкий спектр тем, включая саморазвитие, эмоциональный интеллект, управление стрессом и многое другое. \ud83c\udf1f Воспользуйтесь этими ресурсами для расширения своих знаний и навыков без каких-либо затрат. \ud83d\ude80";

                await SendMessageHandler.SendMessage(telegramBotClient, chat.Id, message, FreeAttachKeyboard,
                    "free-attach.jpg", context);

                break;
            }
            case "test_dm":
            {
                await telegramBotClient.AnswerCallbackQueryAsync(callbackQuery.Id);

                _currentQuestion._currentQuestionIndex = -1;

                var chatId = callbackQuery.Message.Chat.Id;

                await PollHandler.SendNextPoll(chatId, context, _currentQuestion, _questions, telegramBotClient);
                break;
            }
            case "back":
            {
                var msgRepository = new MessageRepository(context);
                var lastMessage = msgRepository.GetLastMessageForChat(chat.Id);

                if (lastMessage != null)
                {
                    // Удаляем предыдущее сообщение по его идентификатору
                    await telegramBotClient.DeleteMessageAsync(chat.Id, lastMessage.MessageId);

                    // Удаляем последнее сообщение из базы данных
                    msgRepository.DeleteLastMessage(lastMessage);
                }
                break;
            }
            case "menu":
            {
                var mainKeyboard = KeyboardHandler.CreateMainKeyboard(telegramBotClient, chat);

                var mainText =
                    $"Привет, {user.FirstName} \ud83d\udc4b\ud83c\udffc Ты попал в Телеграм-бота психолога Тимура Шакирова. \ud83e\udd16 \n\nЕсли ты стремишься снизить тревожность, победить фобии и научиться управлять своими мыслями, я готов тебе помочь. \ud83d\udcac\ud83e\udde0";
                await SendMessageHandler.SendMessage(telegramBotClient, chat.Id, mainText, mainKeyboard, "menu.jpg", context);
                break;
            }
        }
    }


    public static async Task HandlePollAnswer(Update update, ITelegramBotClient _telegramBotClient, DataContext context,
        CurrentQuestion _currentQuestion, List<QuestionAnswerPair> _questions)
    {
        var CounterOfDepression = new Result();
        PollHandler.ProcessPollAnswer(update.PollAnswer, CounterOfDepression);
        var pollId = update.PollAnswer.PollId;
        var chatId = PollHandler.GetChatIdForPoll(pollId, context);
        await PollHandler.SendNextPoll(chatId, context, _currentQuestion, _questions, _telegramBotClient);
    }
}