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

            GameAccount player1 = new GameAccount("Player1");
            BanAccount player2 = new BanAccount("Player2");
            PremiumAccount player3 = new PremiumAccount("Player3");
            GameFactory gameFactory = new GameFactory();

            player2.WinGame(player1, "StandartGame");
            player1.WinGame(player2, "StandartGame");

            player1.LoseGame(player3, "StandartGame");
            player1.LoseGame(player3, "StandartGame");
            player1.LoseGame(player3, "StandartGame");

            player2.WinGame(player1, "TrainingGame");
            player2.WinGame(player3, "TrainingGame");

            player1.GetStats();
            player2.GetStats();
            player3.GetStats();
        }
    }

    class GameAccount
    {
        private List<Game> _historyOfGames = new List<Game>();
        private int _currentRating;

        public string UserName { get; set; }
        public int CurrentRating
        {
            get => _currentRating;
            set => _currentRating = Math.Max(1, value);
        }
        public int GamesCount { get; set; }

        public GameAccount(string userName)
        {
            UserName = userName;
            CurrentRating = 1;
            GamesCount = 0;
        }

        public void WinGame(GameAccount opponent, string gameType)
        {
            RecordGameResult(this, opponent, gameType);
        }

        public void LoseGame(GameAccount opponent, string gameType)
        {
            RecordGameResult(opponent, this, gameType);
        }

        protected void RecordGameResult(GameAccount winner, GameAccount loser, string gameType)
        {
            Game game = GameFactory.CreateGame(gameType, winner, loser);
            int rating = game.Rating;

            winner.UpdateRatingsOnWin(rating);
            loser.UpdateRatingsOnLoss(rating);

            winner._historyOfGames.Add(game);
            loser._historyOfGames.Add(game);
        }

        protected virtual void UpdateRatingsOnWin(int rating)
        {
            CurrentRating += rating;
            GamesCount++;
        }

        protected virtual void UpdateRatingsOnLoss(int rating)
        {
            CurrentRating -= rating;
            GamesCount++;
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

    class BanAccount : GameAccount
    {
        public BanAccount(string userName) : base(userName) { }

        protected override void UpdateRatingsOnLoss(int rating)
        {
            base.UpdateRatingsOnLoss(rating);
            CurrentRating -= rating;
        }
    }

    class PremiumAccount : GameAccount
    {
        private int _countWins = 0;

        public PremiumAccount(string userName) : base(userName) { }

        protected override void UpdateRatingsOnWin(int rating)
        {
            base.UpdateRatingsOnWin(rating);
            _countWins++;

            if (_countWins == 3)
            {
                CurrentRating += 20;
                _countWins = 0;
            }
        }

        protected override void UpdateRatingsOnLoss(int rating)
        {
            base.UpdateRatingsOnLoss(rating);
            _countWins = 0;
        }
    }

    abstract class Game
    {
        private static int _gameIDCounter = 0;

        public int GameID { get; private set; }
        public string Winner { get; private set; }
        public string Loser { get; private set; }
        public int Rating { get; private set; }

        public Game(GameAccount winner, GameAccount loser)
        {
            GameID = ++_gameIDCounter;
            Winner = winner.UserName;
            Loser = loser.UserName;
            Rating = GenerateRating();
        }

        protected abstract int GenerateRating();
    }

    class StandartGame : Game
    {
        private static readonly Random _random = new Random();

        public StandartGame(GameAccount winner, GameAccount loser) : base(winner, loser) { }

        protected override int GenerateRating()
        {
            return _random.Next(20, 101);
        }
    }

    class TrainingGame : Game
    {
        public TrainingGame(GameAccount winner, GameAccount loser) : base(winner, loser) { }

        protected override int GenerateRating()
        {
            return 0;
        }
    }

    class GameFactory
    {
        public static Game CreateGame(string gameType, GameAccount winner, GameAccount loser)
        {
            return gameType switch
            {
                "StandartGame" => new StandartGame(winner, loser),
                "TrainingGame" => new TrainingGame(winner, loser),
                _ => throw new ArgumentException("Invalid game type")
            };
        }
    }
}
