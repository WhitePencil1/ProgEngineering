namespace WebApplication2.Models
{
    public class Room
    {
        private int _nextPlayerId = 0;
        public string Code { get; set; }
        public List<Player> Players = new();
        public Bank Bank { get; set; }
        public int MainPlayerId { get { return (Turn % Players.Count); } }
        public int Turn { get; set; }
        public Room(string code)
        {
            Code = code;
            Bank = new Bank(this);
            Turn = 1;
        }
        public (bool success, string message, int playerId) Join(string playerName, int avatar)
        {
            int newId = _nextPlayerId++;
            Player player = new(playerName, avatar, newId, this);
            Players.Add(player);
            return (true, $"Игрок {playerName} добавлен", player.Id);
        }
        public (bool success, string message) Leave(int playerId)
        {
            Player? player = Players.Find(item => item.Id == playerId);
            return player != null
                ? Players.Remove(player)
                    ? (true, $"игрок {playerId} удалён")
                    : (false, $"ошибка удаления игрока {playerId}")
                : (false, $"игрок {playerId} не найден");
        }
        public Player GetPlayer(int playerId)
        {
            Player? player = Players.Find(item => item.Id == playerId);
            return player;
        }




        public enum Stage
        {
            Start,
            Stage1,
            Stage2,
            Stage3,
            Stage4,
            Stage5,
            Stage6,
            Stage7,
            Stage8,
            Stage90,
            Stage91
        }
        //private bool _isProcessingStage;

        public bool _MainPlayerStart = false;

        private SemaphoreSlim _stageLock = new SemaphoreSlim(1, 1); // Блокировка для одиночного выполнения стадии

        private TaskCompletionSource<bool> _stageCompletionSource = new();

        // Проверяем, завершили ли все игроки текущую стадию
        public bool IsStageComplete()
        {
            return Players.All(player => player.IsStageCompleted);
        }

        // Уведомляем, что стадия завершена
        public bool NotifyStageCompletion(Stage stage)
        {
            bool res = IsStageComplete();
            
            if (res)
            {
                if ((stage == Stage.Start) && (!_MainPlayerStart)) return res;
                ProcessStage(stage); 
            }
            return res;
        }

        // Асинхронное ожидание завершения стадии
        public async Task WaitForStageCompletion()
        {
            await _stageCompletionSource.Task;
        }

        // Сбрасываем состояние всех игроков и флаг стадии
        public void ResetStage()
        {
            foreach (var player in Players)
            {
                player.ResetStage();
            }
            //_isProcessingStage = false; // Сбрасываем флаг обработки
            _stageCompletionSource = new TaskCompletionSource<bool>(); // Новый барьер
        }
        public void ProcessStage(Stage stage)
        {
            // Ожидаем завершения всех действий игроков
            //await WaitForStageCompletion();

            // Используем блокировку, чтобы гарантировать одиночный вызов стадии
            //if (!_isProcessingStage) // Проверка, что стадия еще не обработана
            //{
            //_isProcessingStage = true;

            // Выполняем конкретную логику стадии
            switch (stage)
            {
                case Stage.Start:
                    Start();
                    break;
                case Stage.Stage1:
                    Stage1();
                    break;
                case Stage.Stage2:
                    Stage2();
                    break;
                case Stage.Stage3:
                    Stage3();
                    break;
                case Stage.Stage4:
                    Stage4();
                    break;
                case Stage.Stage5:
                    Stage5();
                    break;
                case Stage.Stage6:
                    Stage6();
                    break;
                case Stage.Stage7:
                    Stage7();
                    break;
                case Stage.Stage8:
                    Stage8();
                    break;
                case Stage.Stage90:
                    Stage90();
                    break;
                case Stage.Stage91:
                    Stage91();
                    break;
                default:
                    throw new InvalidOperationException("Unknown stage");
            }
            _stageCompletionSource.TrySetResult(true); // Уведомляем о завершении
            //ResetStage(); // Подготовка к следующей стадии
        }

        public void Start()
        {
            //логика начала игры
        }
        public void Stage1()//Постоянные издержки.
        {
            foreach (var player in Players)
            {
                player.PayTheCosts();
            }
        }
        public void Stage2()//Определение обстановки на рынке.
        {
            Bank.NewPriceLevel();
        }
        public void Stage3()//Заявки на сырье и материалы.
        {
            Bank.ProcessESMRequests();
        }
        public void Stage4()//Производство продукции.
        {
            //только для синхры таймера

            //foreach (var player in Players)
            //{
            //    foreach (var f in player.actions.FactoriesProcess)
            //    {
            //        player.ProcessESM(f.id, f.esm);
            //    }
            //}
        }
        public void Stage5()//Продажа продукции.
        {
            Bank.ProcessEGPRequests();
        }
        public void Stage6()//Выплата ссудного процента. 
        {
            foreach (var player in Players)
            {
                player.PayProcent();
            }
        }
        public void Stage7()//Погашение ссуд.
        {
            foreach (var player in Players)
            {
                player.PayCredits();
            }
        }
        public void Stage8()//Получение ссуд.
        {
            foreach (var player in Players)
            {
                player.GetCredit(player.actions.FactoryCredit);
            }
        }
        public void Stage90()//Заявки на строительство. 
        {
            foreach (var player in Players)
            {
                foreach (var f in player.actions.FactoriesBuild)
                {
                    player.BuildFactory(f.id, f.auto);
                }
            }
        }
        public void Stage91()//Заявки на улучшение. 
        {
            foreach (var player in Players)
            {
                foreach (var f in player.actions.FactoriesUpgrade)
                {
                    player.UpgradeFactory(f);
                }
            }
            FinalStage();
        }
        public void FinalStage()
        {
            Turn++;
            foreach (var player in Players)
            {
                player.actions.ClearActions();
            }
        }
    }
}
