using System.Collections.Generic;

namespace Clueless.Models
{
    public class Round
    {
        public int Number { get; set; }
        public Player Inquiree { get; set; }
        public Player Shower { get; set; }
        public string Suspect { get; set; }
        public string Weapon { get; set; }
        public string Room { get; set; }
        public bool Solved { get; set; }
    }
}
