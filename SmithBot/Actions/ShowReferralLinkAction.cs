using SmithBot.Database;
using SmithBot.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public class ShowReferralLinkAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            var id = update.Message.From.Id;
            Helpers.Helpers.SetStage(id, 3);
            string link;
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m => m.UserId == id);
                link = user.ReferalUrl;
            }

            await Program.botClient.SendTextMessageAsync(id, String.Format(Helpers.Helpers.GetPhrases(id).yourRefLink, link), replyMarkup: Helpers.Helpers.GetReferralLink(id));
        }
    }
}
