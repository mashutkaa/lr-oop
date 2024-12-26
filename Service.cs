using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Game
{
    interface IPlayerService
    {
        void CreateAccount(GameAccount player);
        List<GameAccount> GetAllAccounts();
        GameAccount? GetAccountByName(string userName);
        void UpdatePlayer(GameAccount player);
        void Delete(string playerName);
    }

    class PlayerService : IPlayerService
    {
        private readonly IRepositoryPlayers _playerRepository;

        public PlayerService(IRepositoryPlayers playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public void CreateAccount(GameAccount player)
        {
            var existingPlayer = _playerRepository.ReadByName(player.UserName);
            if (existingPlayer != null)
            {
                throw new Exception("Гравець з таким ім'ям вже існує.");
            }
            else
            {
                _playerRepository.Create(player);
            }
        }

        public List<GameAccount> GetAllAccounts()
        {
            return _playerRepository.ReadAll();
        }

        public GameAccount? GetAccountByName(string userName)
        {
            return _playerRepository.ReadByName(userName);
        }

        public void UpdatePlayer(GameAccount player)
        {
            _playerRepository.Update(player);
        }

        public void Delete(string playerName)
        {
            var existingPlayer = _playerRepository.ReadByName(playerName);
            if (existingPlayer == null)
            {
                throw new Exception("Гравець з таким ім'ям не існує.");
            }
            _playerRepository.Delete(playerName);
        }
    }

    interface IGameService
    {
        void CreateGame(Game game);
        List<Game> GetAllGames();
        List<Game> GetPlayerGames(string playerName);
        Game ReadByID(int gameID);
        void Update(Game game);
        void Delete(int gameID);
    }

    class GameService : IGameService
    {
        private readonly IRepositoryGames _gameRepository;

        public GameService(IRepositoryGames gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public void CreateGame(Game game)
        {
            _gameRepository.Create(game);
        }

        public List<Game> GetAllGames()
        {
            return _gameRepository.ReadAll();
        }

        public List<Game> GetPlayerGames(string playerName)
        {
            return _gameRepository.ReadByPlayer(playerName);
        }

        public Game ReadByID(int gameID)
        {
            return _gameRepository.ReadByID(gameID);
        }

        public void Update(Game game)
        {
            _gameRepository.Update(game);
        }

        public void Delete(int gameID)
        {
            _gameRepository.Delete(gameID);
        }
    }

    interface IPrintPlayers
    {
        void PrintAllPlayers(List<GameAccount> players);
    }

    class PlayerPrinter : IPrintPlayers
    {
        public void PrintAllPlayers(List<GameAccount> players)
        {
            if (players.Count == 0)
            {
                Console.WriteLine("Немає зареєстрованих гравців.");
                return;
            }

            Console.WriteLine($"{"Ім'я гравця", -20}{"Рейтинг", -10}{"Кількість ігор", -15}");
            Console.WriteLine(new string('-', 50));

            foreach (var player in players)
            {
                Console.WriteLine($"{player.UserName, -20}{player.CurrentRating, -10}{player.GamesCount, -15}");
            }

            Console.WriteLine();
        }
    }

    interface IPrintHistoryGames
    {
        void PrintHistoryGames(List<Game> games);
    }

    class GameHistoryPrinter : IPrintHistoryGames
    {
        public void PrintHistoryGames(List<Game> games)
        {
            if (games.Count == 0)
            {
                Console.WriteLine("Історія ігор порожня.");
                return;
            }

            Console.WriteLine($"{"ID", -5}{"Переможець", -20}{"Переможений", -20}{"Рейтинг гри", -15}");
            Console.WriteLine(new string('-', 70));

            foreach (var game in games)
            {
                Console.WriteLine($"{game.GameID, -5}{game.Winner, -20}{game.Loser, -20}{game.Rating, -15}");
            }

            Console.WriteLine();
        }
    }
}
