using SmithBot.Database;
using SmithBot.Helpers;
using SmithBot.Models;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace SmithBot.Actions
{
    public class CheckAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            await Checks.CheckSubscribeOnChannel(Helpers.Helpers.GetPhrases(update.CallbackQuery.From.Id),update.CallbackQuery.From.Id,Program.botClient,update);
        }

    }
}
