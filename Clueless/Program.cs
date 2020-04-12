using Clueless.Enums;
using Clueless.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Clueless
{
    class Program
    {
        // TODO: Tell them what to show (based on what you already showed and such)
        // TODO: Tell them what to ask
        // TODO: Keep track of what you have showed people
        // TODO: Take notes of rounds (like what other people are asking about and trying to figure out about?
        static void Main(string[] args)
        {
            var game = new Game();

            WelcomePlayer();
            SetUpGame(game);
            PlayTheGame(game);
            EndTheGame();
        }

        // Set Up
        private static void WelcomePlayer()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"-------------------------------------------------------------------------------------------------\n");
            Console.WriteLine($"----------------------          Clue Master Detective Note Taker          -----------------------\n");
            Console.WriteLine($"-------------------------------------------------------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void SetUpGame(Game game)
        {
            OutputToConsoleWithWait($"Gathering Suspects...", 1000);
            OutputToConsoleWithWait($"Collecting Possible Murder Weapons...", 1000);
            OutputToConsoleWithWait($"Compiling List of Possible Crime Scenes...", 1000);
            AddCaseFile(game);
            AddPlayers(game);
            AddPlayersCards(game.Players);
        }

        private static void AddCaseFile(Game game)
        {
            OutputToConsoleWithWait($"Building the Case File...\n", 1000);

            var CaseFile = new Player
            {
                Id = 0,
                Name = "Case File",
                Cards = new List<Card>(Lists.Cards().Select(c => new Card { Id = c.Id, Name = c.Name, Type = c.Type }).ToList()),
                NumberOfCards = 3,
                Type = PlayerType.CaseFile
            };
            game.Players.AddRange(new List<Player> { CaseFile });
        }

        private static void AddPlayers(Game game)
        {
            OutputToConsoleWithWait($"How many detectives are on the case?");
            int numberOfPlayers = GetNumber(3, 8);
            game.Players.AddRange(GetPlayers(numberOfPlayers));
        }

        private static List<Player> GetPlayers(int numberOfPlayers)
        {
            List<Player> addPlayers = new List<Player>();
            var totalNumOfCards = 3;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                var playerType = PlayerType.Opponent;
                if (i == 0)
                {
                    OutputToConsoleWithWait($"\nDetective, what is your name?");
                    playerType = PlayerType.Player;
                }
                else if (i == 1)
                    OutputToConsoleWithWait($"\nGoing to your left, enter the name of detective #{ i + 1 }");
                else
                    OutputToConsoleWithWait($"\nGoing to their left, enter the name of detective #{ i + 1 }");

                string playerName = ValidateNewPlayerNameLength(addPlayers);

                if (i == 0)
                    OutputToConsoleWithWait($"How many cards are in your hand (2 to 10)?");
                else
                    OutputToConsoleWithWait($"How many cards are in their hand (2 to 10)?");

                int numberOfCards = GetNumber(2, 9);
                totalNumOfCards += numberOfCards;

                addPlayers.Add(new Player
                {
                    Id = i + 1,
                    Name = playerName,
                    Cards = new List<Card>(Lists.Cards().Select(c => new Card { Id = c.Id, Name = c.Name, Type = c.Type }).ToList()),
                    NumberOfCards = numberOfCards,
                    Type = playerType
                });
            }

            if (totalNumOfCards != 30)
            {
                DisplayError($"Uh oh by my calculations, that was not enough cards, you may want to restart?");
            }

            return addPlayers;
        }

        private static void AddPlayersCards(List<Player> players)
        {
            var me = players.Where(p => p.Type == PlayerType.Player).First();
            OutputToConsoleWithWait($"\nLet's take note of what you know.");
            for (int i = 0; i < me.NumberOfCards; i++)
            {
                OutputToConsoleWithWait($"Enter card #{ i + 1 }'s name");
                var cardName = GetCardName();
                if (me.Cards.Any(c => c.Name == cardName && c.Status == CardStatus.Yes))
                {
                    DisplayError($"You already entered that card, smh. Try again");
                    cardName = GetCardName();
                }
                CardDiscovery(players, me.Name, cardName);
            }

            foreach (var card in me.Cards.Where(c => c.Status != CardStatus.Yes))
                card.Status = CardStatus.No;
        }

        private static void EndTheGame()
        {
            DisplayInfo($"\nPress any key to end the program.");
            Console.ReadKey();
        }

        // Play the game
        private static void DisplayOptions()
        {
            Console.WriteLine($"\n-------------------------------------------------------------------------------------------------");
            OutputToConsoleWithWait($"\nWhat Would You Like To Do?");
            Console.WriteLine($" 1 - Display Round History");
            Console.WriteLine($" 2 - View Detective Notebook");
            Console.WriteLine($" 3 - Take Note of Another Detective's Inquiry");
            Console.WriteLine($" 4 - Make Your Own Inquiry");
            Console.WriteLine($" 5 - Input a Discovery");
            Console.WriteLine($" 6 - Game is Over");
            Console.Write($"Enter a number: ");
        }

        private static void PlayTheGame(Game game)
        {
            while (!game.IsOver)
            {
                DisplayOptions();
                var selection = Console.ReadLine();
                Console.WriteLine();

                switch (selection)
                {
                    case "1":
                        DisplayRoundHistory(game);
                        break;
                    case "2":
                        DisplayWhatIKnow(game);
                        break;
                    case "3":
                        StartRound(game);
                        MakeDeductions(game);
                        break;
                    case "4":
                        MakeAnInquiry(game);
                        MakeDeductions(game);
                        break;
                    case "5":
                        InquireWhatPlayerHasDiscovered(game);
                        MakeDeductions(game);
                        break;
                    case "6":
                        OutputToConsoleWithWait($"Good Deducing, {game.Players.Where(p => p.Type == PlayerType.Player).First().Name}");
                        game.IsOver = true;
                        break;
                    default:
                        DisplayError($"Could not register selection, please try again");
                        break;
                }

                CheckForGameSolved(game);
            }
        }

        private static void CheckForGameSolved(Game game)
        {
            var caseFileCards = game.Players.Where(p => p.Type == PlayerType.CaseFile).First().Cards.Where(c => c.Status == CardStatus.Yes);
            if (caseFileCards.Count() == 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"-------------------------------------------------------------------------------------------------");
                Console.WriteLine($"*************************************************************************************************");
                Console.WriteLine($"\nThe Case has been solved!");
                Console.Write($"It was {caseFileCards.Where(c => c.Type == CardType.Suspect).First().Name}, ");
                Console.Write($"in the {caseFileCards.Where(c => c.Type == CardType.Room).First().Name}, ");
                Console.Write($"with the {caseFileCards.Where(c => c.Type == CardType.Weapon).First().Name}, ");
                Console.WriteLine($"\nJustice Has Been Served!\n");
                Console.WriteLine($"*************************************************************************************************");
                Console.WriteLine($"-------------------------------------------------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
                DisplayRoundHistory(game);
                DisplayWhatIKnow(game);
                game.IsOver = true;
            }
        }

        // Display Round History
        private static void DisplayRoundHistory(Game game)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"-------------------------------------------------------------------------------------------------\n");
            Console.WriteLine($"-------------------------------          Round History          ---------------------------------\n");
            Console.WriteLine($"-------------------------------------------------------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{"Round",-6}{"Inquiree",-10}{"Suspect",-15} {"Weapon",-15} {"Room",-15} {"Shower",10}");
            foreach (var round in game.Rounds)
            {
                System.Threading.Thread.Sleep(100);
                Console.Write($"{round.Number,-6}{round.Inquiree.Name,-10}");

                foreach (var cardName in new List<string> { round.Suspect, round.Weapon, round.Room })
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (round.Shower.Cards.Where(c => c.Name == cardName).First().Status == CardStatus.Yes)
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    else if (round.Shower.Cards.Where(c => c.Name == cardName).First().Status == CardStatus.No)
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                    else
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"{cardName,-15}");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write($" ");
                }

                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                var showerDetails = "Error?";
                if (round.Shower?.Name != round.Inquiree.Name)
                    showerDetails = round.Shower?.Name;
                else
                    showerDetails = "No One, but the asker," + round.Shower?.Name + ", could have somethings still";
                Console.Write($"{showerDetails,10}\n");
            }
        }

        // Display Game Data
        private static void DisplayWhatIKnow(Game game)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"-------------------------------------------------------------------------------------------------\n");
            Console.WriteLine($"-----------------------          Electronic Detective Notebook          -------------------------\n");
            Console.WriteLine($"-------------------------------------------------------------------------------------------------\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{"",-15}");
            foreach (var player in game.Players)
            {
                Console.Write($"{player.Name,-10}");
            }

            var cardType = CardType.Suspect;
            foreach (var card in Lists.Cards())
            {
                if (cardType != card.Type)
                {
                    Console.WriteLine($"\n-------------------------------------------------------------------------------------------------\n");
                    cardType = card.Type;
                }
                else
                    Console.WriteLine();

                Console.Write($"{card.Name,-15}");
                foreach (var player in game.Players)
                {
                    var cardStatus = player.Cards.Where(c => c.Name == card.Name).First().Status;
                    var cardStatusSymbol = "";
                    var color = ConsoleColor.White;
                    switch (cardStatus)
                    {
                        case CardStatus.Unknown:
                            cardStatusSymbol = "[ ]";
                            break;
                        case CardStatus.Maybe:
                            var roundsInQuestion = game.Rounds
                                .Where(r => !r.Solved)
                                .Where(r => r.Shower.Name == player.Name)
                                .Where(r => r.Suspect == card.Name || r.Weapon == card.Name || r.Room == card.Name)
                                .Select(r => r.Number);
                            var listOfRounds = string.Join($",", roundsInQuestion.Select(n => n.ToString()).ToArray());
                            if (listOfRounds == "")
                                listOfRounds = " ";
                            cardStatusSymbol += $"[{listOfRounds}]";
                            color = ConsoleColor.DarkYellow;
                            break;
                        case CardStatus.No:
                            cardStatusSymbol = "[X]";
                            color = ConsoleColor.DarkRed;
                            break;
                        case CardStatus.Yes:
                            cardStatusSymbol = "[\u25A0]";
                            color = ConsoleColor.DarkGreen;
                            break;
                        default:
                            cardStatusSymbol = "Error";
                            color = ConsoleColor.Red;
                            break;
                    }
                    Console.ForegroundColor = color;
                    Console.Write($"{cardStatusSymbol,-10}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        // Make an Inquiry
        private static void MakeAnInquiry(Game game)
        {
            Round round = new Round
            {
                Number = game.Rounds.Count() + 1,
                Inquiree = game.Players.Where(p => p.Type == PlayerType.Player).FirstOrDefault()
            };

            GetRoundCards(round);
            ExecuteRound(game, round);
        }

        // Player has discovered their own card
        private static void InquireWhatPlayerHasDiscovered(Game game)
        {
            OutputToConsoleWithWait($"\nWhat card have you discovered?");
            var cardName = GetCardName();
            OutputToConsoleWithWait($"Who has it? Or is it in the Case File?");
            var playerName = GetPlayerName(game.Players);
            CardDiscovery(game.Players, playerName, cardName);
            DisplayInfo($"Making note of the Discovery...");
        }

        // Play Round
        private static void StartRound(Game game)
        {
            Round round = new Round();
            GetRoundDetails(game, round);
            ExecuteRound(game, round);
        }

        private static void GetRoundDetails(Game game, Round round)
        {
            round.Number = game.Rounds.Count() + 1;
            GetRoundInquiree(game, round);
            GetRoundCards(round);
        }

        private static void GetRoundInquiree(Game game, Round round)
        {
            Console.WriteLine($"Who is making the inquiry?");
            var inquiree = GetPlayerName(game.Players);
            round.Inquiree = game.Players.Where(p => p.Name == inquiree).FirstOrDefault();
        }

        private static void GetRoundCards(Round round)
        {
            OutputToConsoleWithWait($"What Suspect?");
            var suspect = GetCardName(CardType.Suspect);
            OutputToConsoleWithWait($"What Weapon?");
            var weapon = GetCardName(CardType.Weapon); ;
            OutputToConsoleWithWait($"What Room?");
            var room = GetCardName(CardType.Room); ;

            round.Suspect = suspect;
            round.Weapon = weapon;
            round.Room = room;
        }

        private static void ExecuteRound(Game game, Round round)
        {
            int playerBeingQuestionedId = round.Inquiree.Id;
            while (round.Shower == null)
            {
                if (round.Inquiree.Id == playerBeingQuestionedId + 1)
                {
                    round.Shower = round.Inquiree;
                    SetCardToMaybe(round.Shower.Cards, round.Suspect);
                    SetCardToMaybe(round.Shower.Cards, round.Weapon);
                    SetCardToMaybe(round.Shower.Cards, round.Room);
                    break;
                }
                else if (playerBeingQuestionedId < game.Players.Count() - 1)
                    playerBeingQuestionedId++;
                else
                {
                    if (round.Inquiree.Type == PlayerType.Player)
                    {
                        round.Shower = round.Inquiree;
                        break;
                    }
                    else
                        playerBeingQuestionedId = 1;
                }

                var answer = "";
                var playerBeingQuestioned = game.Players.Where(p => p.Id == playerBeingQuestionedId).First();
                if (playerBeingQuestioned.Type == PlayerType.Player)
                {
                    var cardsToShow = playerBeingQuestioned.Cards
                        .Where(c => (c.Name == round.Suspect || c.Name == round.Weapon || c.Name == round.Room) && c.Status == CardStatus.Yes);

                    if (cardsToShow.Count() > 0)
                    {
                        Console.WriteLine($"What card did you show?");
                        foreach (Card card in cardsToShow)
                        {
                            Console.WriteLine($" - {card.Name}");
                        }
                        GetCardName();
                        answer = "yes";
                    }
                    else
                    {
                        DisplayInfo($"You have nothing to show.");
                        answer = "no";
                    }
                }
                else
                {
                    OutputToConsoleWithWait($"Did {playerBeingQuestioned.Name} show something, Yes or No?");
                    answer = GetYesOrNo();
                }

                if (answer == "yes" || answer == "y")
                {
                    round.Shower = game.Players.Where(p => p.Id == playerBeingQuestionedId).First();

                    if (round.Inquiree.Type == PlayerType.Player)
                    {
                        OutputToConsoleWithWait($"What card did they show you?");
                        round.Solved = true;
                        CardDiscovery(game.Players, round.Shower.Name, GetCardName());
                    }
                    else
                    {
                        if (round.Shower.Type == PlayerType.Player)
                        {
                            round.Solved = true;
                        }
                        else
                        {
                            CheckRoundForDeduction(game.Players, round);
                            SetCardToMaybe(round.Shower.Cards, round.Suspect);
                            SetCardToMaybe(round.Shower.Cards, round.Weapon);
                            SetCardToMaybe(round.Shower.Cards, round.Room);
                        }
                    }
                }
                else if (answer == "no" || answer == "n")
                {
                    var playerThatPassed = game.Players.Where(p => p.Id == playerBeingQuestionedId).First();
                    if (playerThatPassed.Cards.Any(c => (c.Name == round.Suspect || c.Name == round.Weapon || c.Name == round.Room) && c.Status == CardStatus.Yes))
                    {
                        DisplayError($"Okay... are you sure? By my calculations they do.");
                    }
                    else
                    {
                        playerThatPassed.Cards.Where(c => c.Name == round.Suspect).First().Status = CardStatus.No;
                        playerThatPassed.Cards.Where(c => c.Name == round.Weapon).First().Status = CardStatus.No;
                        playerThatPassed.Cards.Where(c => c.Name == round.Room).First().Status = CardStatus.No;
                    }
                }
            }
            game.Rounds.Add(round);
        }

        // Making Deductions
        public static void MakeDeductions(Game game)
        {
            DisplayInfo($"Looking for more Deductions...");
            var wasDeductionMade = false;

            foreach (Player player in game.Players.Where(p => p.Cards.Any(c => c.Status == CardStatus.Maybe || c.Status == CardStatus.Unknown)))
            {
                wasDeductionMade = wasDeductionMade || CheckIfPlayerHasAllTheirCards(player);
            }

            foreach (Round round in game.Rounds.Where(r => !r.Solved && r.Shower.Type != PlayerType.Player))
            {
                CheckForSolvedRounds(round);
                wasDeductionMade = wasDeductionMade || CheckRoundForDeduction(game.Players, round);
            }

            foreach (Card card in Lists.Cards())
            {
                wasDeductionMade = wasDeductionMade || CheckIfOnlyOnePlayerCanHaveCard(game.Players, card);
            }

            wasDeductionMade = wasDeductionMade || CheckIfCaseFileHasDeductions(game.Players);

            if (wasDeductionMade)
            {
                MakeDeductions(game);
            }
        }

        private static bool CheckIfCaseFileHasDeductions(List<Player> players)
        {
            var wasDeductionMade = false;
            wasDeductionMade = wasDeductionMade || CheckIfCaseFileTypeHasOnlyOneOption(players, CardType.Suspect);
            wasDeductionMade = wasDeductionMade || CheckIfCaseFileTypeHasOnlyOneOption(players, CardType.Weapon);
            wasDeductionMade = wasDeductionMade || CheckIfCaseFileTypeHasOnlyOneOption(players, CardType.Room);
            return wasDeductionMade;
        }

        private static bool CheckIfCaseFileTypeHasOnlyOneOption(List<Player> players, CardType type)
        {
            var cardsOfType = players
                .Where(p => p.Type == PlayerType.CaseFile)
                .First().Cards
                    .Where(c => c.Type == type);

            var numOfCardsRuledOut = cardsOfType
                .Where(c => c.Status == CardStatus.No || c.Status == CardStatus.Yes)
                .Count();

            if (numOfCardsRuledOut == cardsOfType.Count() - 1)
            {
                var cardInCasefile = cardsOfType
                    .Where(c => c.Status != CardStatus.No)
                    .First().Name;

                DisplayGreen($"A Deduction Was Made: The Case File Contains {cardInCasefile}");
                CardDiscovery(players, "Case File", cardInCasefile);
                return true;
            }
            return false;
        }

        private static void CheckForSolvedRounds(Round round)
        {
            var isRoundSolved = round.Shower.Cards
                                .Where(c => c.Name == round.Suspect || c.Name == round.Weapon || c.Name == round.Room)
                                .Any(c => c.Status == CardStatus.Yes);

            if (isRoundSolved)
            {
                round.Solved = true;
            }
        }

        private static bool CheckIfPlayerHasAllTheirCards(Player player)
        {
            var numOfYesCards = player.Cards.Where(c => c.Status == CardStatus.Yes).Count();
            if (numOfYesCards == player.NumberOfCards)
            {
                player.Cards
                    .Where(c => c.Status != CardStatus.Yes)
                    .All(c => { c.Status = CardStatus.No; return true; });
                return true;
            }
            else if (numOfYesCards > player.NumberOfCards)
            {
                DisplayError($"It appears that we have marked {player.Name} has more cards than expected?");
            }
            return false;
        }

        private static bool CheckIfOnlyOnePlayerCanHaveCard(List<Player> players, Card card)
        {
            var numOfPlayers = players.Count();
            var numOfNos = players.SelectMany(p => p.Cards)
                .Where(c => c.Name == card.Name && (c.Status == CardStatus.No || c.Status == CardStatus.Yes))
                .Count();

            var numOfPossibleHolders = numOfPlayers - numOfNos;

            if (numOfPossibleHolders == 1)
            {
                var playerWhoHasCard = players
                    .Where(p => p.Cards
                        .Where(c => c.Name == card.Name)
                        .First()
                        .Status != CardStatus.No)
                    .First();

                var cardDiscovered = playerWhoHasCard.Cards
                        .Where(c => c.Name == card.Name)
                        .First();

                cardDiscovered.Status = CardStatus.Yes;
                if (playerWhoHasCard.Type == PlayerType.CaseFile)
                {
                    playerWhoHasCard.Cards
                        .Where(c => c.Status != CardStatus.Yes && c.Type == cardDiscovered.Type)
                        .All(c => { c.Status = CardStatus.No; return true; });
                    DisplayGreen($"A Deduction Was Made: The Case File Contains {cardDiscovered.Name}");
                }
                else
                    DisplayGreen($"A Deduction Was Made: Detective {playerWhoHasCard.Name} has {cardDiscovered.Name}");

                return true;
            }
            return false;
        }

        private static bool CheckRoundForDeduction(List<Player> players, Round round)
        {
            var roundCanBeSolved = NumberOfCardsPlayerDoesntHave(round) == 2
                && round.Inquiree != round.Shower
                && !round.Solved;

            if (roundCanBeSolved)
            {
                round.Solved = true;
                var cardDiscovered = round.Shower.Cards
                    .Where(c => c.Name == round.Room || c.Name == round.Suspect || c.Name == round.Weapon)
                    .Where(c => c.Status != CardStatus.No)
                    .First().Name;

                DisplayGreen($"A Deduction Was Made: {round.Shower.Name} has {cardDiscovered}");

                CardDiscovery(players, round.Shower.Name, cardDiscovered);
                return true;
            }
            return false;
        }

        // Getters
        private static int GetNumber(int min, int max)
        {
            var input = Console.ReadLine();
            while (!Regex.IsMatch(input, @"^\d+$") || Convert.ToInt32(input) < min || Convert.ToInt32(input) > max)
            {
                DisplayError($"That is not a valid input.");
                input = Console.ReadLine();
            }
            return Convert.ToInt32(input);
        }

        private static string ValidateNewPlayerNameLength(List<Player> players)
        {
            var name = Console.ReadLine();
            while (players.Any(c => c.Name == name) || name.Length > 8)
            {
                DisplayError($"Player name must be unique and 8 characters or less.");
                name = Console.ReadLine();
            }
            return name;
        }

        private static string GetPlayerName(List<Player> players)
        {
            var name = Console.ReadLine();
            while (!players.Any(c => c.Name == name))
            {
                DisplayError($"That detective's name is not in my records, try again");
                name = Console.ReadLine();
            }
            return name;
        }

        private static string GetCardName(CardType? type = null)
        {
            var cardName = Console.ReadLine();
            while (!Lists.Cards().Any(c => c.Name == cardName && (type != null ? c.Type == type : true)))
            {
                var cardType = type != null ? type.ToString() : "card";
                DisplayError($"That is not a valid {cardType} name, try again");
                cardName = Console.ReadLine();
            }
            return cardName;
        }

        private static string GetYesOrNo()
        {
            var answer = Console.ReadLine().ToLower();
            while (answer != "no" && answer != "yes" && answer != "n" && answer != "y")
            {
                DisplayError($"Please answer with (Y)es or (N)o");
                answer = Console.ReadLine().ToLower();
            }
            return answer;
        }

        // Helpers
        private static void CardDiscovery(List<Player> players, string playerName, string cardName)
        {
            players
                .Where(p => p.Name == playerName)
                .First().Cards
                    .Where(c => c.Name == cardName)
                    .FirstOrDefault().Status = CardStatus.Yes;

            players
                .Where(p => p.Name != playerName)
                .SelectMany(p => p.Cards)
                    .Where(c => c.Name == cardName)
                    .All(c => { c.Status = CardStatus.No; return true; });

            if (playerName == players.Where(p => p.Type == PlayerType.CaseFile).First().Name)
            {
                var type = Lists.Cards().Where(c => c.Name == cardName).First().Type;

                players.Where(p => p.Type == PlayerType.CaseFile).First().Cards
                    .Where(c => c.Status != CardStatus.Yes && c.Type == type)
                    .All(c => { c.Status = CardStatus.No; return true; });
            }
        }

        private static int NumberOfCardsPlayerDoesntHave(Round round)
        {
            return round.Shower.Cards
                .Where(c => c.Name == round.Room || c.Name == round.Suspect || c.Name == round.Weapon)
                .Where(c => c.Status == CardStatus.No)
                .Count();
        }

        private static void SetCardToMaybe(List<Card> cards, string cardName)
        {
            if (cards.Where(c => c.Name == cardName).First().Status == CardStatus.Unknown)
                cards.Where(c => c.Name == cardName).First().Status = CardStatus.Maybe;
        }

        private static void DisplayInfo(string text)
        {
            DisplaTextWithColor(text, ConsoleColor.Cyan);
        }

        private static void DisplayGreen(string text)
        {
            DisplaTextWithColor(text, ConsoleColor.Green);
        }

        private static void DisplayError(string error)
        {
            DisplaTextWithColor(error, ConsoleColor.Red);
        }

        private static void DisplaTextWithColor(string text, ConsoleColor textColor)
        {
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void OutputToConsoleWithWait(string output, int sleep = 500)
        {
            Console.WriteLine(output);
            System.Threading.Thread.Sleep(sleep);
        }
    }
}
