using Microsoft.AspNetCore.Mvc;
using SmithBot.Actions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SmithBot.Messages
{
    public class MessageExecutor
    {
        public async Task<ActionResult> Execute(Update update, IBaseAction action)
        {
            await action.Start(update);
            return new OkResult();
        }
    }
}
