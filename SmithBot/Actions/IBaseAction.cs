using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public interface IBaseAction
    {
        public Task Start(Update update);
    }
}
