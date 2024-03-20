using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ShaqBot.Handlers;

public class KeyboardHandler
{
    // TO DO: Переделать на полиморфизм
    public static InlineKeyboardMarkup CreateMainKeyboard(ITelegramBotClient botClient, Chat? chat)
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
                        InlineKeyboardButton.WithCallbackData("Тесты", "button1")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Бесплатные материалы", "free-attach"),
                        InlineKeyboardButton.WithUrl("Записаться на консультацию", "https://t.me/ShakirovTimur")
                    }
                });

        return inlineKeyboard;
    }

    public static InlineKeyboardMarkup StartTestKeyboard(ITelegramBotClient botClient, Chat? chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Начать тестирование", "test_dm"),
                InlineKeyboardButton.WithCallbackData("Назад", "back")
            }
        });

        return inlineKeyboard;
    }

    public static InlineKeyboardMarkup ListTestKeyboard(ITelegramBotClient botClient, Chat? chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Тест на уровень депрессии", "test_beck"),
                InlineKeyboardButton.WithCallbackData("Назад", "back")
            }
        });

        return inlineKeyboard;
    }

    public static InlineKeyboardMarkup FreeAttachKeyboard(ITelegramBotClient botClient, Chat? chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(
            new
                List<InlineKeyboardButton
                    []>
                {
                    new[]
                    {
                        InlineKeyboardButton.WithUrl("Видеоуроки",
                            "https://www.youtube.com/@ShakirowTimur"),
                        InlineKeyboardButton.WithCallbackData("Гайды", "button2")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Книги", "books"),
                        InlineKeyboardButton.WithCallbackData("Кейсы", "cases")
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Назад", "back"),
                        InlineKeyboardButton.WithCallbackData("В главное меню", "menu")
                    }
                });

        return inlineKeyboard;
    }
    
    public static InlineKeyboardMarkup BackKeyboard(ITelegramBotClient botClient, Chat? chat)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("В меню", "free-attach")
            }
        });

        return inlineKeyboard;
    }
}