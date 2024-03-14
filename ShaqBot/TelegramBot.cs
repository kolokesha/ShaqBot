using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace ShaqBot;

public class TelegramBot
{
    private readonly IConfiguration _configuration;
    private TelegramBotClient _botClient;

    public TelegramBot(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<TelegramBotClient> GetBot()
    {
        if (_botClient != null)
        {
            return _botClient;
        }
        
        _botClient = new TelegramBotClient($"{_configuration["Token"]}");

        var hook = $"{_configuration["Url"]}api/message/update";
        await _botClient.SetWebhookAsync(hook);
        
        /*_receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                UpdateType.CallbackQuery,
                UpdateType.Poll,
                UpdateType.PollAnswer, // Inline кнопки
                
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true, 
        };*/
        
        /*var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");
        
        await Task.Delay(-1);*/
        
        return _botClient;
    }
}