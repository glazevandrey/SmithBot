using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using SmithBot.Actions;
using SmithBot.Database;
using SmithBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmithBot.Helpers
{
    public class SetWay
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<IBaseAction> GetInline(Telegram.Bot.Types.Update update)
        {
            var phrases = Helpers.GetPhrases(update.CallbackQuery.From.Id);
            IBaseAction type;
            List<IBaseAction> result = new List<IBaseAction>();
            if(update.CallbackQuery.Data == "check")
            {
                type = new CheckAction();
                result.Add(type);
                type = new SetReferralAction();
                result.Add(type);
            }
            else if (update.CallbackQuery.Data == phrases.referrals)
            {
                Helpers.SetStage(update.CallbackQuery.From.Id, 5);
                type = new ShowReferralsInfoAction();
                result.Add(type);
            }



            return result;
        }

        public List<IBaseAction> Get(Telegram.Bot.Types.Update update)
        {
            IBaseAction type;
            List<IBaseAction> result = new List<IBaseAction>();
            
            if (update.Message.Text.StartsWith("/start"))
            {
                type = new StartAction();
                result.Add(type);
                return result;
            }

            var user = new BotUser();
            using (var db = new UserContext())
            {
                user = db.BotUsers.FirstOrDefault(m => m.UserId == update.Message.From.Id);
            }
            if (user == null)
            {
                return null;
            }
            var phrases = Helpers.GetPhrases(update.Message.From.Id);
            if (update.Message.Text.ToLower() == phrases.refSystem.ToLower())
            {
                type = new ShowReferralLinkAction();
                result.Add(type);
            }
            else if (update.Message.Text.ToLower() == phrases.information.ToLower())
            {
                type = new ShowMainInfoAction();
                result.Add(type);
            }
            else if ((update.Message.Text.ToLower() == phrases.profile.ToLower() || user.Stage == 4) && update.Message.Text.ToLower() != phrases.toMainMenu.ToLower())
            {
                if (string.IsNullOrEmpty(user.Wallet))
                {
                    type = new SetWalletAction();
                    result.Add(type);
                }
                
                type = new ProfileAction();
                result.Add(type);
            }
            else if (update.Message.Text.ToLower() == phrases.toMainMenu.ToLower())
            {
                type = new ToMainMenuAction();
                result.Add(type);
            }
            

            return result;
        }

    }
}
