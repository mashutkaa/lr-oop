using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
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
