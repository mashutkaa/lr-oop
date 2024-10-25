using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;

            GameAccount ghostRider = new GameAccount("GhostRider");
            GameAccount iceBlade = new GameAccount("IceBlade");

            ghostRider.WinGame(iceBlade, 20);
            iceBlade.LoseGame(ghostRider, 20);
            iceBlade.WinGame(ghostRider, 30);
            iceBlade.LoseGame(ghostRider, 20);
            iceBlade.WinGame(ghostRider, 30);

            ghostRider.GetStats();
            iceBlade.GetStats();
        }
    }

    class GameAccount
    {
        private static int _gameIDCounter = 0;
        private List<Game> _historyOfGames = new List<Game>();
        private int _currentRating;

        public string UserName { get; set; }
        public int CurrentRating
        {
            get
            {
                return _currentRating;
            }
            set
            {
                _currentRating = (value < 1) ? 1 : value;
            }
        }
        public int GamesCount { get; set; }

        public GameAccount(string userName)
        {
            UserName = userName;
            CurrentRating = 1;
            GamesCount = 0;
        }

        public void WinGame(GameAccount opponent, int rating)
        {
            if (rating < 0)
            {
                throw new Exception("Від'ємний рейтинг");
            }

            this.CurrentRating += rating;
            opponent.CurrentRating -= rating;

            var game = new Game(this.UserName, opponent.UserName, rating);
            this._historyOfGames.Add(game);
            opponent._historyOfGames.Add(game);

            this.GamesCount++;
            opponent.GamesCount++;
        }

        public void LoseGame(GameAccount opponent, int rating)
        {
            if (rating < 0)
            {
                throw new Exception("Від'ємний рейтинг");
            }

            this.CurrentRating -= rating;
            opponent.CurrentRating += rating;

            var game = new Game(opponent.UserName, this.UserName, rating);
            this._historyOfGames.Add(game);
            opponent._historyOfGames.Add(game);

            this.GamesCount++;
            opponent.GamesCount++;
        }

        public void GetStats()
        {
            Console.WriteLine($"Історія ігор гравця {UserName}:");
            Console.WriteLine($"{"Номер гри",-15}{"Опонент",-20}{"Результат",-15}{"Рейтинг",-10}");
            Console.WriteLine(new string('-', 60));

            foreach (Game game in _historyOfGames)
            {
                string result = game.Winner == UserName ? "Перемога" : "Програш";
                Console.Write($"{game.GameID,-15}");
                Console.Write($"{(game.Winner == UserName ? game.Loser : game.Winner),-20}");
                Console.Write($"{result,-15}");
                Console.Write($"{game.Rating,-10}");
                Console.WriteLine();
            }

            int numberOfWins = _historyOfGames.Count(game => game.Winner == UserName);
            int numberOfLosses = _historyOfGames.Count(game => game.Loser == UserName);

            Console.WriteLine();
            Console.WriteLine($"Статистика гравця {UserName}:");
            Console.WriteLine($"Кількість ігор: {GamesCount}");
            Console.WriteLine($"Поточний рейтинг: {CurrentRating}");
            Console.WriteLine($"Кількість перемог: {numberOfWins}");
            Console.WriteLine($"Кількість поразок: {numberOfLosses}");
            Console.WriteLine();
        }
    }

    class Game
    {
        private static int _gameIDCounter = 0;

        public int GameID { get; private set; }
        public string Winner { get; private set; }
        public string Loser { get; private set; }
        public int Rating { get; private set; }

        public Game(string winner, string loser, int rating)
        {
            GameID = ++_gameIDCounter;
            Winner = winner;
            Loser = loser;
            Rating = rating;
        }
    }
}
