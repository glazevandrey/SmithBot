using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using SmithBot.Bot;
using SmithBot.Database;
using SmithBot.Helpers;
using SmithBot.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SmithBot
{
    public class Program
    {
        public static Telegram.Bot.TelegramBotClient botClient;
        public static ValidateRequest validator;
        public static IBotService botService;
        public static Config config;
        public static string AdminWallet;
        public static User Me;
        public static int TotalBalance;
        public static DateTime startTime = DateTime.Now;
        public static DateTime endTime = new DateTime();

        public static Dictionary<int, double> referralLevels = new Dictionary<int, double>()
        {
            [0] = 0.10,
            [1] = 0.05,
            [2] = 0.02,
        };

        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        public static void Main(string[] args)
        {
            SetConfig();
            CreateHostBuilder(args).Build().Run();
        }
        private static void SetConfig()
        {

            string jsonString = System.IO.File.ReadAllText("config.json", Encoding.UTF8);
            var config = System.Text.Json.JsonSerializer.Deserialize<Config>(jsonString);
        
            Program.botClient = new Telegram.Bot.TelegramBotClient(config.BotToken);
            Program.config = config;
            Program.validator = new ValidateRequest();
            Program.Me = Program.botClient.GetMeAsync().Result;
            Program.endTime = DateTime.ParseExact(config.EndDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            Program.AdminWallet = config.AdminWallet;
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
