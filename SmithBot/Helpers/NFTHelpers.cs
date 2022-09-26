using SmithBot.Database;
using SmithBot.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;

namespace SmithBot.Helpers
{
    public class NFTHelpers
    {
        public static NewNFT GetNftByNumber(int num)
        {
            var nft = new NewNFT();
            using (var db = new UserContext())
            {
                nft = db.NFTs.FirstOrDefault(m=>m.Number == num);
            }
            if(nft == null)
            {
                return null;
            }

            return nft;
        }
        public static NewNFT GetMostCloserNFT(List<NewNFT> list)
        {
            var result = new NewNFT();
            var allNft = new List<NewNFT>();

            using (var db = new UserContext())
            {
                allNft = db.NFTs.Where(m => m.OwnerWallet != Program.AdminWallet).ToList();
            }

            if (list != null)
            {
                allNft = list;
            }

            allNft = allNft.OrderBy(m => m.Amount).ToList();
            var result2 = new NewNFT();

            //  min
            foreach (var item in allNft)
            {
                if (item.Amount <= Program.TotalBalance)
                {
                    result = item;
                }
                else
                {
                    if (result.Name == null)
                    {
                        result = item;
                    }
                    break;
                }
            }

            allNft.Reverse();
            // max
            foreach (var item in allNft)
            {
                if (item.Amount >= Program.TotalBalance)
                {
                    result2 = item;
                }
                else
                {
                    break;
                }
            }
            var r_1 = Math.Abs(result.Amount - Program.TotalBalance);
            var r_2 = Math.Abs(result2.Amount - Program.TotalBalance);
            if (r_1 > r_2)
            {
                return result2;
            }
            else
            {
                return result;
            }
        }
        public static string GetWinnerNFT()
        {
            var result = new NewNFT();
            var result2 = new NewNFT();

            var allNft = new List<NewNFT>();
            using (var db = new UserContext())
            {
                allNft = db.NFTs.ToList();
            }
            var correctedBalance1 = Program.TotalBalance;
            var correctedBalance2 = Program.TotalBalance;

            allNft = allNft.OrderBy(m => m.Amount).ToList();
            foreach (var item in allNft)
            {

                if (item.Amount <= correctedBalance1)
                {
                    result = item;
                }
                else
                {
                    break;
                }

            }

            allNft.Reverse();

            // max
            foreach (var item in allNft)
            {

                if (item.Amount >= correctedBalance2)
                {
                    result2 = item;
                }
                else
                {
                    break;
                }
            }
            var r_1 = Math.Abs(result.Amount - correctedBalance1);
            var r_2 = Math.Abs(result2.Amount - correctedBalance2);
            if (r_1 > r_2)
            {
                return result2.Name;
            }
            else
            {
                return result.Name;
            }
        }
        public static void CorrelateTelegramAndWallet(long id, string wallet)
        {
            var nfts = new List<NewNFT>();
            using (var db = new UserContext())
            {
                nfts = db.NFTs.Where(m => m.OwnerWallet == wallet).ToList();
            }

            if (nfts == null || nfts.Count == 0)
            {
                return;
            }

            using (var db = new UserContext())
            {
                foreach (var item in nfts)
                {
                    if (item.OwnerTelegramId == 0)
                    {
                        item.OwnerTelegramId = id;
                    }
                    db.Update(item);

                }
                db.SaveChanges();
            }

        }
        public static int CalculateAmount(int num)
        {
            int sum = 0;
            for (int i = 1; i <= num; i++)
            {
                sum += i;
            }
            return sum;
        }
        public static List<NewNFT> HaveWinnerNFT(long telegramId)
        {
            var pool = new List<NewNFT>();

            using (var db = new UserContext())
            {
                pool = db.NFTs.Where(m => m.OwnerTelegramId == telegramId).ToList();
            }

            var closed = GetMostCloserNFT(null);

            var winnerPool = new List<NewNFT>();

            foreach (var item in pool)
            {
                if (item.Number >= closed.Number)
                {
                    winnerPool.Add(item);
                }
            }

            return winnerPool;
        }
        public static List<NewNFT> GetTwoLowestFreeNFT()
        {
            var list = new List<NewNFT>();

            using (var db = new UserContext())
            {
                list = db.NFTs.Where(m => m.OwnerWallet == Program.AdminWallet).ToList();
            }

            list = list.OrderBy(m => m.Number).ToList();
            return list.Take(2).ToList();
        }

        public static ResponseNFTSearch GetNFTWithCursor(string cursor)
        {
            string result = "";
            var handler = new HttpClientHandler();

            using (var httpClient = new HttpClient(handler))
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.getgems.io/graphql"))
                {
                    request.Headers.TryAddWithoutValidation("authority", "api.getgems.io");
                    request.Headers.TryAddWithoutValidation("accept", "*/*");
                    request.Headers.TryAddWithoutValidation("accept-language", "ru,en;q=0.9");
                    request.Headers.TryAddWithoutValidation("origin", "https://getgems.io");
                    request.Headers.TryAddWithoutValidation("referer", "https://getgems.io/");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua", "^^");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
                    request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "^^");
                    request.Headers.TryAddWithoutValidation("sec-fetch-dest", "empty");
                    request.Headers.TryAddWithoutValidation("sec-fetch-mode", "cors");
                    request.Headers.TryAddWithoutValidation("sec-fetch-site", "same-site");
                    request.Headers.TryAddWithoutValidation("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.167 YaBrowser/22.7.3.829 Yowser/2.5 Safari/537.36");
                    request.Headers.TryAddWithoutValidation("x-auth-token", "");

                    var json = File.ReadAllText("json.json");
                    json = json.Replace("-adress-", Program.config.CollectionAdress);
                    GetGemsRequest myDeserializedClass = JsonConvert.DeserializeObject<GetGemsRequest>(json);
                    myDeserializedClass.variables.cursor = cursor;

                    request.Content = new StringContent(JsonConvert.SerializeObject(myDeserializedClass));

                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = httpClient.SendAsync(request).Result;
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            try
            {
                var res = JsonConvert.DeserializeObject<ResponseNFTSearch>(result);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
