using DotNetEnv;
using TelegramBot;

Env.Load();

string token = Environment.GetEnvironmentVariable("TOKEN") ?? "";
var bot = new Bot(token);

bot.Start().Wait();