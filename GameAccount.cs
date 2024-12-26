using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
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

        public void WinGame(GameAccount opponent, Game game)
        {
            RecordGameResult(this, opponent, game);
        }

        public void LoseGame(GameAccount opponent, Game game)
        {
            RecordGameResult(opponent, this, game);
        }

        protected void RecordGameResult(GameAccount winner, GameAccount loser, Game game)
        {
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
            Console.WriteLine($"{"ID гри",-15}{"Опонент",-20}{"Результат",-15}{"Рейтинг",-10}");
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

    class RestrictionsAccount : GameAccount
    {
        public RestrictionsAccount(string userName) : base(userName) { }

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
}
