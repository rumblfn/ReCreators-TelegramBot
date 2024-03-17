using DotNetEnv;
using TelegramBot;
using TelegramBot.Utils.Logger;

Logger.Info("Program started.");

// Use for specify correct environment variables.
string envPath = ".env.prod";
#if DEBUG
    envPath = ".env.dev";
#endif

// Load env from specified path.
Env.Load(envPath);

string token = Environment.GetEnvironmentVariable("TOKEN") ?? "";
var bot = new Bot(token);

bot.Start().Wait();

Logger.Info("Program finished.");