using SmithBot.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using SmithBot.Database;
using System.Linq;
using Telegram.Bot;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using System.Net;
using System.Threading;
using System.Reflection.PortableExecutable;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace SmithBot.Helpers
{
    public class Helpers
    {
        public static List<List<BotUser>> CalculateRefferals(long id)
        {
            var user = new BotUser();

            List<BotUser> first = new List<BotUser>();

            List<BotUser> second = new List<BotUser>();

            List<BotUser> third = new List<BotUser>();

            List<List<BotUser>> res = new List<List<BotUser>>();

            using (var db = new UserContext())
            {
                user = db.BotUsers.FirstOrDefault(m => m.UserId == id);


                var currentReferralBotUser = user;

                first = db.BotUsers.Where(m => m.Referrer.UserId == id).ToList();

                foreach (var item in first)
                {
                    second.AddRange(db.BotUsers.Where(m => m.Referrer.UserId == item.UserId).ToList());

                    foreach (var _item in second)
                    {
                        third.AddRange(db.BotUsers.Where(m => m.Referrer.UserId == _item.UserId).ToList());
                    }
                }
            }

            res.Add(first);
            res.Add(second);
            res.Add(third);

            return res;
        }
        public static void SetWallet(long id, string wallet)
        {
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == id);
                user.Wallet = wallet;
                db.SaveChangesAsync();
            }
        }
        public static InlineKeyboardMarkup GetReferralLink(long id)
        {
            var link = "";
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == id);
                if (user == null)
                {
                    return null;
                }

                link = user.ReferalUrl;
            }

            var shareButton = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithSwitchInlineQuery(Helpers.GetPhrases(id).share, query: link),
                    InlineKeyboardButton.WithCallbackData(Helpers.GetPhrases(id).referrals),
                },
            });

            return shareButton;
        }
        public static ReplyKeyboardMarkup CreateKeyboard(long id)
        {
            int stage;
            var markup = new ReplyKeyboardMarkup(new[] { new KeyboardButton("") });

            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == id);
                if (user == null)
                {
                    return null;
                }

                stage = user.Stage;
            }

            switch (stage)
            {
                case 1:
                    markup = new ReplyKeyboardMarkup(
                      new[]
                      {
                            new KeyboardButton(Helpers.GetPhrases(id).information),
                            new KeyboardButton(Helpers.GetPhrases(id).instriction),
                            new KeyboardButton(Helpers.GetPhrases(id).refSystem)
                      }
                      );

                    break;
                case 2:
                    markup = new ReplyKeyboardMarkup(
                        new[]
                        {
                            new KeyboardButton(Helpers.GetPhrases(id).profile),
                            new KeyboardButton(Helpers.GetPhrases(id).toMainMenu),
                        }
                    );
                    break;
                case 3:
                    markup = new ReplyKeyboardMarkup(
                        new[]
                        {
                            new KeyboardButton(Helpers.GetPhrases(id).referrals),
                            new KeyboardButton(Helpers.GetPhrases(id).purchases),
                            new KeyboardButton(Helpers.GetPhrases(id).toMainMenu),

                        }
                    );
                    break;
                case 4:
                    markup = new ReplyKeyboardMarkup(
                    new[]
                    {
                            new KeyboardButton(Helpers.GetPhrases(id).toMainMenu),
                    }
                );
                    break;
                case 6:
                    markup = new ReplyKeyboardMarkup(
                   new[]
                   {
                            new KeyboardButton(Helpers.GetPhrases(id).toMainMenu),
                   }
               );

                    break;
                case 7:

                    markup = new ReplyKeyboardMarkup(
                   new[]
                   {
                            new KeyboardButton(Helpers.GetPhrases(id).toMainMenu),
                   }
               );

                    break;
                default:
                    break;
            }

            markup.ResizeKeyboard = true;

            return markup;
        }
        public static void SetStage(long id, int stage)
        {
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == id);
                if (user == null)
                {
                    return;
                }
                user.Stage = stage;
                db.SaveChanges();
            }
        }
        public static async Task ActivateReferral(UserContext db, BotUser referralBotUser)
        {
            const int referralLevels = 3;

            var currentReferralBotUser = referralBotUser;

            for (int i = 1; i <= referralLevels; i++)
            {

                db
                   .Entry(currentReferralBotUser)
                   .Reference(u => u.Referrer)
                   .Load();

                if (currentReferralBotUser is null || currentReferralBotUser.Referrer is null)
                {
                    break;
                }

                var referralPercent = Program.referralLevels[i];
                currentReferralBotUser.Referrer.Balance += referralPercent;
                db.Update(currentReferralBotUser.Referrer);
                currentReferralBotUser = currentReferralBotUser.Referrer;

                try
                {
                    await Program.botClient.SendTextMessageAsync(
                        currentReferralBotUser.Referrer!.UserId,
                        String.Format(Helpers.GetPhrases(currentReferralBotUser.UserId).referralActivated, referralBotUser.Username)
                    );
                }
                catch (ApiRequestException)
                {
                    continue;
                }
                catch (NullReferenceException ex)
                {
                    await Program.botClient.SendTextMessageAsync(
                        currentReferralBotUser.UserId,
                         String.Format(Helpers.GetPhrases(currentReferralBotUser.UserId).referralActivated, referralBotUser.Username)
                    );
                }
            }
        }
        public static async Task ShowReferralsInfo(long id)
        {
            var all = Helpers.CalculateRefferals(id);

            List<BotUser> first = all[0];

            List<BotUser> second = all[1];

            List<BotUser> third = all[2];

            BotUser user = new BotUser();

            string _first = Helpers.GetStringRef(first);

            string _second = Helpers.GetStringRef(second);

            string _third = Helpers.GetStringRef(third);

            await Program.botClient.SendTextMessageAsync(id,
                String.Format(Helpers.GetPhrases(id).referralsInfo, (_first == "") ? "0" : _first, (_second == "") ? "0" : _second, (_third == "") ? "0" : _third));

        }
        public static Phrases GetPhrases(long id)
        {
            return new Phrases();
            var language = "";
            using (var db = new UserContext())
            {
                try
                {
                    language = db.BotUsers.FirstOrDefault(m => m.UserId == id).Language;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            if (language == null)
            {
                return new Phrases();
            }
            string jsonString = "";
            var files = new DirectoryInfo(Environment.CurrentDirectory + "/Languages/").GetFiles();
            if (language.ToLower() == "russian")
            {
                return new Phrases();
            }

            var lang_name = "";
            foreach (var item in files)
            {
                if (item.Name.Replace(".json", string.Empty).ToLower() == language.ToLower())
                {
                    lang_name = item.Name;
                }
            }
            jsonString = File.ReadAllText(Environment.CurrentDirectory + "/Languages/" + lang_name);

            var phrases = System.Text.Json.JsonSerializer.Deserialize<Phrases>(jsonString);
            return phrases;
        }
        public static string GetStringRef(List<BotUser> users)
        {
            var res = "";
            using (var db = new UserContext())
            {
                foreach (var item in users)
                {

                    res += String.Format(item.Username);

                }
            }

            return res.TrimEnd('\n');
        }
        public static void SendMoneyToReferrals(UserContext db, BotUser user)
        {
            var currentReferralBotUser = user;

            for (int i = 0; i < Program.referralLevels.Count; i++)
            {
                db
                    .Entry(currentReferralBotUser)
                    .Reference(u => u.Referrer)
                    .Load();

                if (currentReferralBotUser is null || currentReferralBotUser.Referrer is null)
                {
                    break;
                }

                var referralCoins = Program.referralLevels[i];
                var reward = referralCoins;

                currentReferralBotUser.Referrer.Balance += reward;
                db.Update(currentReferralBotUser.Referrer);
                currentReferralBotUser = currentReferralBotUser.Referrer;

            }
        }

    }
}
