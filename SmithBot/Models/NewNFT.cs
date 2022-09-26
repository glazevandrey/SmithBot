using System.ComponentModel.DataAnnotations;

namespace SmithBot.Models
{
    public class NewNFT
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public long OwnerTelegramId { get; set; }
        public string OwnerWallet { get; set; }
        public double Amount { get; set; }
        public double Number { get; set; }
        public string Address { get; set; }
    }
}
