using ShaqBot.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ShaqBot.Handlers;

public class SendMessageHandler
{
    public static async Task SendMessage(ITelegramBotClient telegramBotClient, ChatId chatId, string text,
        InlineKeyboardMarkup keyboardmarkup, string photo, DataContext context)
    {
        var path = $"../ShaqBot/Images/{photo}";

        var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        var message = await telegramBotClient.SendPhotoAsync(
            chatId,
            replyMarkup: keyboardmarkup,
            photo: InputFile.FromStream(fileStream, "questions.jpg"),
            caption: text);
        

        var lastMessage = new LastMessage
        {
            ChatId = message.Chat.Id,
            MessageId = message.MessageId,
            DateSent = DateTime.Now
        };

        var msgRepository = new MessageRepository(context);
        msgRepository.SaveLastMessage(lastMessage);
    }
}