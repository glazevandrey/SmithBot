using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using SmithBot.Database;
using SmithBot.Models;
using System;
using Telegram.Bot.Types.Enums;
using System.Linq;

namespace SmithBot.Helpers
{
    public class Checks
    {
        public static async Task CheckSubscribeOnChannel(Phrases phrases, long id, Telegram.Bot.ITelegramBotClient client, Update update)
        {
            var member = await client.GetChatMemberAsync(Program.config.Channel, id);

            if (member is null || member.Status == ChatMemberStatus.Kicked || member.Status == ChatMemberStatus.Left)
            {
                await Checks.CheckSubscribe(update, client);
                return;
            }

            var markup = new ReplyKeyboardMarkup(
          new[]
          {
                    new KeyboardButton(phrases.information),
                    new KeyboardButton(phrases.instriction),
                    new KeyboardButton(phrases.refSystem)
          }
      );

            markup.ResizeKeyboard = true;
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == id);
                user.Subscriber = true;
                db.SaveChanges();
            }
            await client.SendTextMessageAsync(
                id,
                String.Format(phrases.registered),
                replyMarkup: markup
            );

        }

        public static async Task CheckSubscribe(Update update, ITelegramBotClient client)
        {
            var channelUrl = $"https://t.me/{Program.config.Channel.TrimStart('@')}";
            var id = update.Message.From.Id;
            var startMarkup = new InlineKeyboardMarkup(new[]
            {
                new []
                {
                    InlineKeyboardButton.WithUrl(
                        string.Format(Helpers.GetPhrases(id).goToChannel, Program.config.Channel),
                        channelUrl
                    )
                },
                new []
                {
                    InlineKeyboardButton.WithCallbackData(
                        Helpers.GetPhrases(id).check,
                        "check")

                }
            });

            await client.SendTextMessageAsync(id, string.Format(Helpers.GetPhrases(id).subscribeOnChannel, Program.config.Channel), replyMarkup: startMarkup);
        }

    }
}
