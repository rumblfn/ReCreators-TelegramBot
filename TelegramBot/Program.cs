using DotNetEnv;
using TelegramBot;

Env.Load();

var bot = new Bot
{
    Token = Environment.GetEnvironmentVariable("TOKEN") ?? "",
};

bot.Init().Wait();