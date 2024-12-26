using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    interface IRepositoryPlayers
    {
        void Create(GameAccount player);
        GameAccount? ReadByName(string playerName);
        List<GameAccount> ReadAll();
        void Update(GameAccount player);
        void Delete(string playerName);
    }

    class RepositoryPlayers : IRepositoryPlayers
    {
        private readonly DBContext _context;

        public RepositoryPlayers(DBContext context)
        {
            _context = context;
        }

        public void Create(GameAccount account)
        {
            _context.Accounts.Add(account);
        }

        public GameAccount? ReadByName(string accountName)
        {
            return _context.Accounts.FirstOrDefault(a => a.UserName == accountName);
        }

        public List<GameAccount> ReadAll()
        {
            return _context.Accounts;
        }

        public void Update(GameAccount account)
        {
            var existingAccount = ReadByName(account.UserName);
            if (existingAccount != null)
            {
                _context.Accounts.Remove(existingAccount);
                _context.Accounts.Add(account);
            }
            else
            {
                throw new Exception("Гравець з таким ім'ям не існує.");
            }
        }

        public void Delete(string accountName)
        {
            _context.Accounts.RemoveAll(a => a.UserName == accountName);
        }
    }

    interface IRepositoryGames
    {
        void Create(Game game);
        Game ReadByID(int gameID);
        List<Game> ReadByPlayer(string playerName);
        List<Game> ReadAll();
        void Update(Game game);
        void Delete(int gameID);
    }

    class RepositoryGames : IRepositoryGames
    {
        private readonly DBContext _context;

        public RepositoryGames(DBContext context)
        {
            _context = context;
        }

        public void Create(Game game)
        {
            _context.HistoryOfGames.Add(game);
        }

        public Game ReadByID(int gameID)
        {
            var game = _context.HistoryOfGames.FirstOrDefault(g => g.GameID == gameID);
            if (game != null)
            {
                return game;
            }
            else
            {
                throw new Exception($"Гра з ID {game} не знайдена.");
            }
        }

        public List<Game> ReadByPlayer(string playerName)
        {
            return _context.HistoryOfGames.Where(g => g.Winner == playerName || g.Loser == playerName).ToList();
        }

        public List<Game> ReadAll()
        {
            return _context.HistoryOfGames;
        }

        public void Update(Game game)
        {
            var existingGame = ReadByID(game.GameID);
            if (existingGame != null)
            {
                _context.HistoryOfGames.Remove(existingGame);
                _context.HistoryOfGames.Add(game);
            }
            else
            {
                throw new Exception($"Гра з ID '{game.GameID}' не знайдена.");
            }
        }

        public void Delete(int gameId)
        {
            _context.HistoryOfGames.RemoveAll(g => g.GameID == gameId);
        }
    }
}
