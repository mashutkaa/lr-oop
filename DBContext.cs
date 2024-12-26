using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class DBContext
    {
        public List<GameAccount> Accounts { get; set; } = new List<GameAccount>();
        public List<Game> HistoryOfGames { get; set; } = new List<Game>();

        public DBContext()
        {
            seed();
        }

        public void seed()
        {
            Accounts.Add(new GameAccount("ghostRider"));
            Accounts.Add(new RestrictionsAccount("wildFairy"));
            Accounts.Add(new PremiumAccount("iceBlade"));
        }
    }
}
