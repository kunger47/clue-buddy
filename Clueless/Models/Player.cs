using Clueless.Enums;
using System.Collections.Generic;

namespace Clueless.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Card> Cards { get; set; }
        public int NumberOfCards { get; set; }
        public PlayerType Type { get; set; }
    }
}
