using Clueless.Enums;

namespace Clueless.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CardType Type { get; set; }
        public CardStatus Status { get; set; }
    }
}
