using Microsoft.AspNetCore.Mvc;
using SmithBot.Database;
using SmithBot.Helpers;
using SmithBot.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Update = Telegram.Bot.Types.Update;

namespace SmithBot.Actions
{
    public class ProfileAction : IBaseAction
    {
        Update _update;
        BotUser _user;
        public async Task Start(Update update)
        {
            var user = new BotUser();
            using (var db = new UserContext())
            {
                user = db.BotUsers.FirstOrDefault(m=>m.UserId == update.Message.From.Id);
            }

            _update = update;
            _user = user;

            if (string.IsNullOrEmpty(user.Wallet))
            {
                await Task.CompletedTask;
            }

            //Есть ли НФТ с шансом на победу
            var winnerList = Helpers.NFTHelpers.HaveWinnerNFT(update.Message.From.Id);
            
            if (winnerList.Count == 0)
            {
                await SendZeroChances();
                return;
            }
            else
            {

                var winner = await IsWinner(winnerList);
                if (!string.IsNullOrEmpty(winner.Name))
                {
                    await SendWinnerNft(winner.Name);
                    return;
                }

                await SendProfileText(winnerList);
            }
        }

        public async Task SendProfileText(List<NewNFT> list) =>
            await Program.botClient.SendTextMessageAsync(_user.UserId, await GetProfileText(list),
                replyMarkup: Helpers.Helpers.CreateKeyboard(_update.Message.From.Id),
                parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        private async Task<string> GetProfileText(List<NewNFT> list)
        {

            string result = "Ваши НФТ имеющие шансы на победу:\n\n";
            foreach (var item in list)
            {
                result += item.Name + "\n";
            }

            var closer = Helpers.NFTHelpers.GetMostCloserNFT(list);

            result += $"\n{closer.Name} имеет самые большие шансы на победу.";
            result += $"\nЧтобы приблизить его к победе, Вы можете увеличить банк, купив НФТ с меньшим номером.\nНапример:\n";
            var twoLoset = Helpers.NFTHelpers.GetTwoLowestFreeNFT();
            foreach (var item in twoLoset)
            {
                result += $@"<a href=""https://getgems.io/collection/{Program.config.CollectionAdress}/{item.Address}/"">{item.Name}</a>";
                result += "\n";
            }

            return result;
        }
        private async Task SendWinnerNft(string name) =>
            await Program.botClient.SendTextMessageAsync(_user.UserId, $"Ваш НФТ {name} побеждает",
                replyMarkup: Helpers.Helpers.CreateKeyboard(_update.Message.From.Id));
        private async Task SendZeroChances()
        {
            var text = "К сожалению, сейчас у Вас нету НФТ которые имеют шанс на победу.\nПобедные НФТ начинаются с номера: " + Helpers.NFTHelpers.GetMostCloserNFT(null).Number +
                $"\n\nВы можете приобрести ближайшие НФТ:" +
                $"\n<a href=\"https://getgems.io/collection/" + Program.config.CollectionAdress + "/" + NFTHelpers.GetNftByNumber((int)Helpers.NFTHelpers.GetMostCloserNFT(null).Number + 1).Address + "/\">" + NFTHelpers.GetNftByNumber((int)Helpers.NFTHelpers.GetMostCloserNFT(null).Number + 1).Name + "</a>" + "\n" +
                $"<a href=\"https://getgems.io/collection/" + Program.config.CollectionAdress + "/" + NFTHelpers.GetNftByNumber((int)Helpers.NFTHelpers.GetMostCloserNFT(null).Number + 2).Address + "/\">" + NFTHelpers.GetNftByNumber((int)Helpers.NFTHelpers.GetMostCloserNFT(null).Number + 2).Name + "</a>" + "\n";
            Helpers.Helpers.SetStage(_update.Message.From.Id, 7);
            await Program.botClient.SendTextMessageAsync(_user.UserId,
               text,
                replyMarkup: Helpers.Helpers.CreateKeyboard(_update.Message.From.Id), parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
        private async Task<NewNFT> IsWinner(List<NewNFT> list)
        {
            var winnerNft = Helpers.NFTHelpers.GetWinnerNFT();
            var mostCloseer = Helpers.NFTHelpers.GetMostCloserNFT(null);
            bool winned = false;

            var win = new NewNFT();
            foreach (var item in list)
            {
                if (item.Name == winnerNft)
                {
                    winned = true;
                    win = item;
                }
                else if (item.Name == mostCloseer.Name)
                {
                    winned = true;
                    win = item;
                }

            }

            return win;
        }

    }
}
