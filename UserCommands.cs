using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    interface ICommand
    {
        void Execute();
        void DisplayCommandInfo();
    }

    // виведення списку усіх гравців
    class DisplayAllPlayersCommand : ICommand
    {
        private readonly IPlayerService _playerService;
        private readonly IPrintPlayers _playerPrinter;

        public DisplayAllPlayersCommand(IPlayerService playerService, IPrintPlayers playerPrinter)
        {
            _playerService = playerService;
            _playerPrinter = playerPrinter;
        }

        public void Execute()
        {
            var players = _playerService.GetAllAccounts();
            _playerPrinter.PrintAllPlayers(players);
        }

        public void DisplayCommandInfo()
        {
            Console.WriteLine("Відобразити список усіх створених гравців.");
        }
    }

    // Команда для додавання нового гравця
    class AddPlayerCommand : ICommand
    {
        private readonly IPlayerService _playerService;

        public AddPlayerCommand(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Execute()
        {
            Console.Write("Введіть ім'я нового гравця: ");
            string userName = Console.ReadLine();

            string accountType = "Standard";
            Console.Write("Введіть тип облікового запису\n1 - Standard\n2 - Premium\n3 - Ban: ");
            int choice = int.Parse(Console.ReadLine());

            switch(choice)
            {
                case 1:
                    accountType = "Standard";
                    break;
                case 2:
                    accountType = "Premium";
                    break;
                case 3:
                    accountType = "Restriction";
                    break;
                default:
                    Console.WriteLine("Такого типу акаунту не існує");
                    break;
            }  

            var newPlayer = AccountFactory.CreateAccount(userName, accountType);
            try
            {
                _playerService.CreateAccount(newPlayer);
                Console.WriteLine($"Гравець {userName} успішно доданий як {accountType}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        public void DisplayCommandInfo()
        {
            Console.WriteLine("Додати нового гравця.");
        }
    }

    // Команда для відображення статистики гравця
    class DisplayPlayerStatsCommand : ICommand
    {
        private readonly IPlayerService _playerService;

        public DisplayPlayerStatsCommand(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        public void Execute()
        {
            Console.Write("Введіть ім'я гравця для відображення статистики: ");
            string playerName = Console.ReadLine();

            var player = _playerService.GetAccountByName(playerName);
            if (player == null)
            {
                Console.WriteLine($"Гравця з іменем {playerName} не знайдено.");
                return;
            }

            player.GetStats();
        }

        public void DisplayCommandInfo()
        {
            Console.WriteLine("Відобразити статистику гравця.");
        }
    }

    // Команда для запуску гри
    class PlayGameCommand : ICommand
    {
        private readonly GameEngine _gameEngine;
        private readonly IPlayerService _playerService;

        public PlayGameCommand(GameEngine gameEngine, IPlayerService playerService)
        {
            _gameEngine = gameEngine;
            _playerService = playerService;
        }

        public void Execute()
        {
            // Отримання імені гравців від користувача
            Console.Write("Введіть ім'я першого гравця: ");
            string player1Name = Console.ReadLine();

            Console.Write("Введіть ім'я другого гравця: ");
            string player2Name = Console.ReadLine();

            string gameType;


            Console.Write("Введіть тип гри:\n1 - StandartGame,\n2 - TrainingGame: ");
            int choice = int.Parse(Console.ReadLine());

            if (choice == 1)
            {
                gameType = "StandartGame";
            } else
            {
                gameType = "TrainingGame";
            }

            // Отримання акаунтів гравців або їх створення, якщо не знайдені
            var player1 = _playerService.GetAccountByName(player1Name);
            var player2 = _playerService.GetAccountByName(player2Name);

            try
            {
                // Симуляція гри
                _gameEngine.SimulateGame(player1, player2, gameType);
                Console.WriteLine($"Гра між {player1Name} і {player2Name} успішно завершена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час гри: {ex.Message}");
            }
        }

        public void DisplayCommandInfo()
        {
            Console.WriteLine("Запустити гру.");
        }
    }

    // виведення історії всіх ігор
    class DisplayGameHistoryCommand : ICommand
    {
        private readonly IGameService _gameService;
        private readonly IPrintHistoryGames _gameHistoryPrinter;

        public DisplayGameHistoryCommand(IGameService gameService, IPrintHistoryGames gameHistoryPrinter)
        {
            _gameService = gameService;
            _gameHistoryPrinter = gameHistoryPrinter;
        }

        public void Execute()
        {
            var games = _gameService.GetAllGames();
            _gameHistoryPrinter.PrintHistoryGames(games);
        }

        public void DisplayCommandInfo()
        {
            Console.WriteLine("Відобразити історію всіх ігор.");
        }
    }

}
