using SmithBot.Database;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore;
using SmithBot.Models;
using Telegram.Bot.Exceptions;
using SmithBot.Helpers;
using Telegram.Bot.Types.Enums;

namespace SmithBot.Actions
{
    public class StartAction : IBaseAction
    {
        public async Task Start(Telegram.Bot.Types.Update update)
        {
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == update.Message.From.Id);
                if (user == null)
                {
                    var message = update.Message!;

                    var files = new DirectoryInfo(Environment.CurrentDirectory + "/Languages/").GetFiles();

                    //var rows = new List<InlineKeyboardButton[]>();
                    //var cols = new List<InlineKeyboardButton>();

                    //for (int i = 0; i < files.Length; i++)
                    //{
                    //    var name = files[i].Name.Replace(".json", string.Empty);
                    //    cols.Add(InlineKeyboardButton.WithCallbackData(name));
                    //    rows.Add(cols.ToArray());
                    //    cols = new List<InlineKeyboardButton>();

                    //    if (i + 1 >= files.Length)
                    //    {
                    //        cols.Add(InlineKeyboardButton.WithCallbackData("Русский", "russian"));
                    //        rows.Add(cols.ToArray());
                    //        cols = new List<InlineKeyboardButton>();
                    //    }
                    //}

                    //var markup = new InlineKeyboardMarkup(rows);

                    //await Program.botClient.SendTextMessageAsync(
                    //    message.Chat.Id,
                    //    String.Format("Выберите язык:"),
                    //    replyMarkup: markup
                    //);

                    var startStringParts = message.Text!.Split(" ");

                    if (startStringParts.Length == 2)
                    {
                        var referrerUserIdString = startStringParts[1];

                        if (long.TryParse(referrerUserIdString, out long referrerUserId))
                        {
                            await CreateBotUser(referrerUserId, message.From!);
                        }
                        else
                        {
                            await CreateBotUser(null, message.From!);
                        }
                    }
                    else
                    {
                        await this.CreateBotUser(null, message.From!);
                    }


                    await Checks.CheckSubscribe
                        (update, Program.botClient);

                }
            }
        }



        public async Task CreateBotUser(long? referrerUserId, User referralUser)
        {
            using var db = new UserContext();

            BotUser? referrerBotUser = null;

            if (!(referrerUserId is null) && referrerUserId != referralUser.Id)
            {
                referrerBotUser = await db.BotUsers
                    .Where(u => u.UserId == referrerUserId)
                    .FirstOrDefaultAsync();
            }

            var botUser = new BotUser
            {
                UserId = referralUser.Id,
                Username = referralUser.Username,
                Referrer = referrerBotUser,
                RegistrationDate = DateTime.Now
            };

            try
            {
                await db.AddAsync(botUser);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return;
            }
        }

        public async Task ActivateReferral(UserContext db, BotUser referralBotUser)
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
                db.Update(currentReferralBotUser.Referrer);
                currentReferralBotUser = currentReferralBotUser.Referrer;

                try
                {
                    await Program.botClient.SendTextMessageAsync(
                        currentReferralBotUser.Referrer!.UserId,
                        String.Format(Helpers.Helpers.GetPhrases(currentReferralBotUser.UserId).referralActivated, referralBotUser.Username)
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
                         String.Format(Helpers.Helpers.GetPhrases(currentReferralBotUser.UserId).referralActivated, referralBotUser.Username)
                    );
                }
            }
        }

    }
}
