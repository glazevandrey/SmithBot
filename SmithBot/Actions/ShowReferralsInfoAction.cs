using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public class ShowReferralsInfoAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            await Helpers.Helpers.ShowReferralsInfo(update.CallbackQuery.From.Id);
        }
    }
}
