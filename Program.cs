using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;

            // симуляція бд
            DBContext dbContext = new DBContext();

            // об'єкти репозиторію гри та сервісу гри
            RepositoryGames gameRepo = new RepositoryGames(dbContext);
            GameService gameService = new GameService(gameRepo);

            // гравці: об'єкти репозиторію та сервісу
            RepositoryPlayers accountRepo = new RepositoryPlayers(dbContext);
            PlayerService playerService = new PlayerService(accountRepo);

            GameAccount player1 = AccountFactory.CreateAccount("Гравець1");
            GameAccount player2 = AccountFactory.CreateAccount("Гравець2", "Ban");
            GameAccount player3 = AccountFactory.CreateAccount("Гравець3", "Premium");

            GameAccount ghostRider = accountRepo.ReadByName("ghostRider");


            // Створення об'єкта класу GameEngine
            GameEngine gameEngine = new GameEngine(gameService, playerService);

            gameEngine.SimulateGame(player1, player2, "StandartGame");
            gameEngine.SimulateGame(player2, player3, "TrainingGame");
            gameEngine.SimulateGame(player3, player2, "StandartGame");

            player1.GetStats();
            player2.GetStats();
            player3.GetStats();

            // отримуємо всі акаунти та ігри з сервісу
            var allPlayers = playerService.GetAllAccounts();
            var allGames = gameService.GetAllGames();

            PlayerPrinter playerPrinter = new PlayerPrinter();
            GameHistoryPrinter gamePrinter = new GameHistoryPrinter();

            playerPrinter.PrintAllPlayers(allPlayers);
            gamePrinter.PrintHistoryGames(allGames);
        }

    }

    class GameEngine
    {
        private Random _random;
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;

        public GameEngine(GameService gameService, PlayerService playerService)
        {
            _random = new Random();
            _gameService = gameService;
            _playerService = playerService;
        }

        public void SimulateGame(GameAccount player1, GameAccount player2, string gameType)
        {
            if (_playerService.GetAccountByName(player1.UserName) == null)
            {
                _playerService.CreateAccount(player1);
            }

            if (_playerService.GetAccountByName(player2.UserName) == null)
            {
                _playerService.CreateAccount(player2);
            }

            bool player1Wins = _random.Next(2) == 0;
            Game game;

            if (player1Wins)
            {
                game = GameFactory.CreateGame(gameType, player1, player2);
                player1.WinGame(player2, game);
            }
            else
            {
                game = GameFactory.CreateGame(gameType, player2, player1);
                player1.LoseGame(player2, game);
            }

            _gameService.CreateGame(game);
        }


    }


}