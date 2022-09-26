using Microsoft.Extensions.DependencyInjection;
using NLog;
using SmithBot.Database;
using SmithBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SmithBot.Quartz
{
    public class QuartzService : IQuartzService
    {
        public Task SetNewBalanceJob()
        {
            int cursor = 0;
            var list = new List<Edge>();
            var current = Helpers.NFTHelpers.GetNFTWithCursor(cursor.ToString());
            list.AddRange(current.data.alphaNftItemSearch.edges);
            cursor = list.Count;
            for (; ;)
            {
                cursor += DoWork(ref list, ref current, cursor);
                
                if(current.data.alphaNftItemSearch.info.hasNextPage == false)
                {
                    break;
                }
            }

            var mapped = Mapping(list);

            ////SIMULATION
            //using (var db = new UserContext())
            //{
            //    mapped = db.NFTs.ToList();
            //}


            int sum = 0;

            using (var db = new UserContext())
            {
                foreach (var item in mapped)
                {
                    if(item.OwnerWallet != Program.AdminWallet)
                    {
                        sum += (int)item.Number;
                    }

                    var dbNft = db.NFTs.FirstOrDefault(m => m.Name == item.Name && m.Amount == item.Amount);
                    if (dbNft != null)
                    {
                        dbNft.OwnerWallet = item.OwnerWallet;
                        continue;
                    }
                    db.NFTs.Add(item);
                }
                db.SaveChanges();
            }

            Program.TotalBalance = sum;
            
            return Task.CompletedTask;
        }
        private List<NewNFT> Mapping(List<Edge> list)
        {
            var result = new List<NewNFT>();
            foreach (var item in list)
            {
                var current = new NewNFT();
                current.Name = item.node.name;
                current.OwnerWallet = item.node?.owner?.wallet;
                using (var db = new UserContext())
                {
                    if (db.BotUsers.FirstOrDefault(m => m.Wallet == current.OwnerWallet) != null)
                    {
                        current.OwnerTelegramId = db.BotUsers.FirstOrDefault(m => m.Wallet == current.OwnerWallet).UserId;
                    }
                    else
                    {
                        current.OwnerTelegramId = 0;
                    }
                }
                current.Number = CalculateNumber(current.Name);
                current.Address = item.node.address;
                current.Amount = Helpers.NFTHelpers.CalculateAmount((int)current.Number);
                result.Add(current);
            }
            return result;
        }

        // ВАЖНО: переписывается под каждый отдельный кошелек
        private int CalculateNumber(string name)
        {
            Regex reg = new Regex(@"(\w+)\s#(.+)");

            var numStr = reg.Match(name);
            if (numStr == null)
            {
                return 0;
            }

            var num = Convert.ToInt32(numStr.Groups[2].Value);
            return num;
        }

        private int DoWork(ref List<Edge> list, ref ResponseNFTSearch current, int cursor)
        {
            current = Helpers.NFTHelpers.GetNFTWithCursor( cursor.ToString());
            if(current.data == null)
            {
                return list.Count;
            }
            list.AddRange(current.data.alphaNftItemSearch.edges);
            return current.data.alphaNftItemSearch.edges.Count;
        }
    }
}
