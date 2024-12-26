using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
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
}
