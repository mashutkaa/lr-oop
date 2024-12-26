using Game;
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

            DBContext dbContext = new DBContext();

            RepositoryGames gameRepo = new RepositoryGames(dbContext);
            GameService gameService = new GameService(gameRepo);

            RepositoryPlayers playerRepo = new RepositoryPlayers(dbContext);
            PlayerService playerService = new PlayerService(playerRepo);

            GameEngine gameEngine = new GameEngine(gameService, playerService);

            PlayerPrinter playerPrinter = new PlayerPrinter();
            GameHistoryPrinter gamePrinter = new GameHistoryPrinter();

            List<ICommand> commands = new List<ICommand>
            {
                new DisplayAllPlayersCommand(playerService, playerPrinter),
                new AddPlayerCommand(playerService),
                new DisplayPlayerStatsCommand(playerService),
                new PlayGameCommand(gameEngine, playerService),
                new DisplayGameHistoryCommand(gameService, gamePrinter),
            };

            while(true)
            {
                Console.WriteLine("Ваш вибір (або 0 для виходу): ");
                for (int i = 0; i < commands.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    commands[i].DisplayCommandInfo();
                }

                int choice = int.Parse(Console.ReadLine());

                if (choice > 0 && choice <= commands.Count)
                {
                    commands[choice - 1].Execute();
                } else if (choice == 0)
                {
                    Console.WriteLine("Завершення роботи.");
                    break;
                }
                else
                {
                    Console.WriteLine("Некоректний вибір. Спробуйте ще раз.");
                    continue;
                }

            }
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