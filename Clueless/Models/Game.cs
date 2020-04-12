using System;
using System.Collections.Generic;
using System.Text;

namespace Clueless.Models
{
    class Game
    {
        public Game()
        {
            Players = new List<Player>();
            Rounds = new List<Round>();
        }

        public bool IsOver { get; set; }
        public List<Player> Players { get; set; }
        public List<Round> Rounds { get; set; }
    }
}
