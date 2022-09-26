using SmithBot.Database;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public class ToMainMenuAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            Helpers.Helpers.SetStage(update.Message.From.Id, 1);

            await Program.botClient.SendTextMessageAsync(update.Message.From.Id,
                Helpers.Helpers.GetPhrases(update.Message.From.Id).youInMainMenu,
                replyMarkup: Helpers.Helpers.CreateKeyboard(update.Message.From.Id));
        }
    }
}
