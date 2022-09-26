using SmithBot.Database;
using SmithBot.Models;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public class SetWalletAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            var user = new BotUser();
            using (var db = new UserContext())
            {
                user = db.BotUsers.FirstOrDefault(m => m.UserId == update.Message.From.Id);
            }
            // если это второй заход в это действие (пользователь ввел свой кошелек)
            if (user.Stage == 4)
            {
                Helpers.Helpers.SetStage(update.Message.From.Id, 6);
                Helpers.Helpers.SetWallet(update.Message.From.Id, update.Message.Text);
                Helpers.NFTHelpers.CorrelateTelegramAndWallet(update.Message.From.Id, update.Message.Text);

                await Program.botClient.SendTextMessageAsync(user.UserId, "Вы успешно установили новый адрес кошелька!", replyMarkup: Helpers.Helpers.CreateKeyboard(update.Message.From.Id));
            }
            else
            {
                Helpers.Helpers.SetStage(user.UserId, 4);
                await Program.botClient.SendTextMessageAsync(user.UserId, Helpers.Helpers.GetPhrases(user.UserId).setWalletPls);
            }
        }
    }
}
