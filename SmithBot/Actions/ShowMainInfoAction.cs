using SmithBot.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public class ShowMainInfoAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            using (var db = new UserContext())
            {
                var user = db.BotUsers.FirstOrDefault(m=>m.UserId == update.Message.From.Id);
                user.Stage = 2;
                db.SaveChanges();
            }

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id,
                String.Format(Helpers.Helpers.GetPhrases(update.Message.From.Id).mainInfo, Program.TotalBalance, Convert.ToInt32(Program.TotalBalance - (Program.TotalBalance * 0.3)), Helpers.NFTHelpers.GetMostCloserNFT(null).Name, Helpers.NFTHelpers.GetWinnerNFT(), Program.endTime.ToString("dd.MM.yyyy"),
                Convert.ToInt32(TimeSpan.FromTicks((Program.endTime - DateTime.Now).Ticks).Days),
                Convert.ToInt32(TimeSpan.FromTicks((Program.endTime-DateTime.Now).Ticks).Hours),
                Convert.ToInt32(TimeSpan.FromTicks((Program.endTime - DateTime.Now).Ticks).Minutes),
                Convert.ToInt32(TimeSpan.FromTicks((Program.endTime - DateTime.Now).Ticks).Seconds)
                ),
                replyMarkup : Helpers.Helpers.CreateKeyboard(update.Message.From.Id), parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}
