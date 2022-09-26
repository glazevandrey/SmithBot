using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace SmithBot.Models
{
    public class BotUser
    {
        [Key]
        public long UserId { get; set; }

        public string? Username { get; set; }
        public double Balance { get; set; } = 0;
        public string Language { get; set; } = "russian";
        public int Stage { get; set; } = 0;

        public string ReferalUrl { get; set; }
        public List<BotUser> Referrals { get; set; } = new List<BotUser>();
        public BotUser? Referrer { get; set; } = null;

        public bool Activated { get; set; } = false;
        public bool Subscriber { get; set; }

        public string Wallet { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

}
