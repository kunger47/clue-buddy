using Clueless.Enums;
using Clueless.Models;
using System.Collections.Generic;

static class Lists
{
    public static List<Card> Cards()
    {
        return new List<Card>
        {
            new Card { Id = 0, Name = "Col. Mucus", Type = CardType.Suspect },
            new Card { Id = 1, Name = "Prof. Puke", Type = CardType.Suspect },
            new Card { Id = 2, Name = "Mr. Gross", Type = CardType.Suspect },
            new Card { Id = 3, Name = "Mrs. Pneumonia", Type = CardType.Suspect },
            new Card { Id = 4, Name = "Miss Scurvy", Type = CardType.Suspect },
            new Card { Id = 5, Name = "Mrs. Wash", Type = CardType.Suspect },
            new Card { Id = 6, Name = "Mme Rash", Type = CardType.Suspect },
            new Card { Id = 7, Name = "Sgt. Germ", Type = CardType.Suspect },
            new Card { Id = 8, Name = "M. Bactieria", Type = CardType.Suspect },
            new Card { Id = 9, Name = "Miss Pinkeye", Type = CardType.Suspect },

            new Card { Id = 10, Name = "Handshake", Type = CardType.Weapon },
            new Card { Id = 11, Name = "Sneeze", Type = CardType.Weapon },
            new Card { Id = 12, Name = "Kiss", Type = CardType.Weapon },
            new Card { Id = 13, Name = "Cough", Type = CardType.Weapon },
            new Card { Id = 14, Name = "Touch Face", Type = CardType.Weapon },
            new Card { Id = 15, Name = "Cruise Ship", Type = CardType.Weapon },
            new Card { Id = 16, Name = "Lick", Type = CardType.Weapon },
            new Card { Id = 17, Name = "Hug", Type = CardType.Weapon },

            new Card { Id = 18, Name = "China", Type = CardType.Room },
            new Card { Id = 19, Name = "USA", Type = CardType.Room },
            new Card { Id = 20, Name = "Norway", Type = CardType.Room },
            new Card { Id = 21, Name = "Italy", Type = CardType.Room },
            new Card { Id = 22, Name = "Brazil", Type = CardType.Room },
            new Card { Id = 23, Name = "Russia", Type = CardType.Room },
            new Card { Id = 24, Name = "Canada", Type = CardType.Room },
            new Card { Id = 25, Name = "Egypt", Type = CardType.Room },
            new Card { Id = 26, Name = "South Africa", Type = CardType.Room },
            new Card { Id = 27, Name = "Japan", Type = CardType.Room },
            new Card { Id = 28, Name = "Austrialia", Type = CardType.Room },
            new Card { Id = 29, Name = "Hawaii", Type = CardType.Room },
        };

        //return new List<Card>
        //{
        //    new Card { Id = 0, Name = "Col. Mustard", Type = CardType.Suspect },
        //    new Card { Id = 1, Name = "Prof. Plum", Type = CardType.Suspect },
        //    new Card { Id = 2, Name = "Mr. Green", Type = CardType.Suspect },
        //    new Card { Id = 3, Name = "Mrs. Peacock", Type = CardType.Suspect },
        //    new Card { Id = 4, Name = "Miss Scarlet", Type = CardType.Suspect },
        //    new Card { Id = 5, Name = "Mrs. White", Type = CardType.Suspect },
        //    new Card { Id = 6, Name = "Mme Rose", Type = CardType.Suspect },
        //    new Card { Id = 7, Name = "Sgt. Gray", Type = CardType.Suspect },
        //    new Card { Id = 8, Name = "M. Brunette", Type = CardType.Suspect },
        //    new Card { Id = 9, Name = "Miss Peach", Type = CardType.Suspect },

        //    new Card { Id = 10, Name = "Knife", Type = CardType.Weapon },
        //    new Card { Id = 11, Name = "Candlestick", Type = CardType.Weapon },
        //    new Card { Id = 12, Name = "Revolver", Type = CardType.Weapon },
        //    new Card { Id = 13, Name = "Rope", Type = CardType.Weapon },
        //    new Card { Id = 14, Name = "Lead Pipe", Type = CardType.Weapon },
        //    new Card { Id = 15, Name = "Wrench", Type = CardType.Weapon },
        //    new Card { Id = 16, Name = "Poison", Type = CardType.Weapon },
        //    new Card { Id = 17, Name = "Horseshoe", Type = CardType.Weapon },

        //    new Card { Id = 18, Name = "Courtyard", Type = CardType.Room },
        //    new Card { Id = 19, Name = "Gazebo", Type = CardType.Room },
        //    new Card { Id = 20, Name = "Drawing Room", Type = CardType.Room },
        //    new Card { Id = 21, Name = "Dining Room", Type = CardType.Room },
        //    new Card { Id = 22, Name = "Kitchen", Type = CardType.Room },
        //    new Card { Id = 23, Name = "Carriage House", Type = CardType.Room },
        //    new Card { Id = 24, Name = "Trophy Room", Type = CardType.Room },
        //    new Card { Id = 25, Name = "Conservatory", Type = CardType.Room },
        //    new Card { Id = 26, Name = "Studio", Type = CardType.Room },
        //    new Card { Id = 27, Name = "Billiard Room", Type = CardType.Room },
        //    new Card { Id = 28, Name = "Library", Type = CardType.Room },
        //    new Card { Id = 29, Name = "Fountain", Type = CardType.Room },
        //};
    }

}