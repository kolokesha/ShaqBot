# Psychologist Telegram Bot for Depression Test

This Telegram bot is designed to provide psychological support and administer the Beck Depression Inventory (BDI) test to users. The bot is built using ASP.NET Core and integrates with the Telegram Bot API.

## Features

- Administer the Beck Depression Inventory (BDI) test to users.
- Provide psychological support and guidance.
- Send reminders and notifications for self-care activities.

## Installation

1. Clone this repository to your local machine.
    
    bashCopy code
    
    `git clone https://github.com/yourusername/psychologist-telegram-bot.git`
    
2. Install the required dependencies using NuGet Package Manager or the .NET CLI.
    
    bashCopy code
    
    `dotnet restore`
    
3. Configure the bot settings in the `appsettings.json` file.
    
    jsonCopy code
    
    `{   "BotSettings": {     "Token": "YOUR_TELEGRAM_BOT_TOKEN",     "WebhookUrl": "YOUR_PUBLIC_WEBHOOK_URL"   } }`
    
4. Deploy the bot to a server or use a local development environment.
    
5. Set up webhooks for your bot using the Telegram Bot API. Replace `<your_token>` with your bot token and `<your_webhook_url>` with your public URL.
    
    bashCopy code
    
    `curl -X POST "https://api.telegram.org/bot<your_token>/setWebhook?url=<your_webhook_url>"`
    
6. Start the ASP.NET Core application.
    
    bashCopy code
    
    `dotnet run`
    
7. Interact with your bot on Telegram!
    

## Usage

1. Start a conversation with the bot by searching for its username on Telegram.
2. Follow the bot's prompts to take the depression test or seek psychological support.

## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License.
