using Microsoft.EntityFrameworkCore;
using SmithBot.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace SmithBot.Actions
{
    public class SetReferralAction : IBaseAction
    {
        public async Task Start(Update update)
        {
            string referralUrlFmt = "https://t.me/{0}?start={1}";

            var referralUrl = String.Format(referralUrlFmt, Program.Me.Username, update.Id);

            using (var db = new UserContext())
            {
                var botUser = await db.BotUsers
                    .Where(u => u.UserId == update.CallbackQuery.From.Id)
                    .FirstOrDefaultAsync();

                if (!(botUser is null) && !botUser.Activated)
                {
                    botUser.Activated = true;
                    botUser.ReferalUrl = referralUrl;
                    botUser.Stage = 1;
                    await Helpers.Helpers.ActivateReferral(db, botUser);
                    db.Update(botUser);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
