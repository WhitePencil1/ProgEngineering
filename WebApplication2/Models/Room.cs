using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using static WebApplication2.GameSettings;

namespace WebApplication2.Models
{
    public class Room
    {
        private int _nextPlayerId = 0;
        public string Code { get; set; }
        public List<Player> Players = new();
        public Bank Bank { get; set; }

        public int MainPlayerId { get { if (Players.Count != 0) return ((Turn-1) % Players.Count); else return -1; } }

        public int Turn { get; set; }
        public List<string> Log { get; private set; } = new List<string>();
        public void AddLog(string message)
        {
            Log.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
        }
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
            AddLog($"{player.Name} присоединился к комнате.");
            return (true, $"Игрок {playerName} добавлен", player.Id);
        }
        public bool Leave(int playerId)
        {
            Player? player = Players.Find(item => item.Id == playerId);

            if (player != null)
            {
                if (Players.Remove(player))
                {
                    AddLog($"игрок id = {playerId} ({player.Name}) удалён");
                    return true;
                }
                else
                {
                    AddLog($"ошибка удаления игрока id = {playerId}");
                    return false;
                }
            }
            else
            {
                AddLog($"игрок id = {playerId} не найден");
                return false;
            }
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
            AddLog("Игра началась");
            AddLog($"Ход № {Turn} начался:");
            AddLog($"{Players[MainPlayerId].Name} - старший игрок на этот ход.");
            //логика начала игры
        }
        public void Stage1()//Постоянные издержки.
        {
            AddLog($"Ход № {Turn} начался:");
            AddLog("Стадия 1: Выплата издержек.");
            foreach (var player in Players)
            {
                (int esm, int egp, int factory, int total, bool defaulter) = player.PayTheCosts();
                AddLog($"{player.Name} заплатил издержек на {total}$. Из них {esm}$ за ЕСМ, {egp}$ за ЕГП, {factory}$ за фабрики.");
            }
        }
        public void Stage2()//Определение обстановки на рынке.
        {
            Bank.NewPriceLevel();
            AddLog("Стадия 2: Определение обстановки на рынке.");
            AddLog($"Банк определил новый уровень цен - {Bank.Level}.");
            AddLog($"Стоимость ЕСМ: {Bank.ESMPrice}$, Количество ЕСМ: {Bank.ESMCount}.");
            AddLog($"Стоимость ЕГП: {Bank.EGPPrice}$, Количество ЕГП: {Bank.EGPCount}.");
            AddLog("Стадия 3: Обработка заявок на сырьё и материалы");
        }
        public void Stage3()//Заявки на сырье и материалы.
        {
            Dictionary<Player, (int count, int price)> result = Bank.ProcessESMRequests();
            foreach (var player in result.Keys)
            {
                AddLog($"Заявка {player.Name}: {player.actions.RequestedESM.count} ЕСМ за {player.actions.RequestedESM.price}$.");
                AddLog($"{player.Name} приобрёл {result[player].count} ЕСМ за {result[player].price}$.");
            }
            AddLog("Стадия 4: Производство продукции.");
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
            AddLog("Стадия 5: Продажа продукции.");
        }
        public void Stage5()//Продажа продукции.
        {
            Dictionary<Player, (int count, int price)> result = Bank.ProcessEGPRequests();
            foreach (var player in result.Keys)
            {
                AddLog($"Заявка {player.Name}: {player.actions.RequestedESM.count} ЕГП за {player.actions.RequestedESM.price}$.");
                AddLog($"{player.Name} продал {result[player].count} ЕГП за {result[player].price}$.");
            }
        }
        public void Stage6()//Выплата ссудного процента. 
        {
            AddLog("Стадия 6: Выплата ссудного процента.");
            foreach (var player in Players)
            {
                (bool success, int sum) = player.PayProcent();
                if (success)
                {
                    if (sum != 0)
                    {
                        AddLog($"{player.Name} успешно выплатил проценты по кредиту на сумму {sum}$.");
                    }
                    else AddLog($"{player.Name} не имеет кредитов.");
                }
                else AddLog($"{player.Name} не смог выплатить проценты по кредиту на сумму {sum}$.");
            }
        }
        public void Stage7()//Погашение ссуд.
        {
            AddLog("Стадия 7: Выплата ссуд.");
            foreach (var player in Players)
            {
                (bool success, int sum) = player.PayCredits();
                if (success)
                {
                    if (sum == 0)
                    {
                        AddLog($"{player.Name} не имеет задолженностей по кредитам на этот месяц."); 
                    }
                    else AddLog($"{player.Name} успешно выплатил кредит на сумму {sum}$.");
                }
                else AddLog($"{player.Name} не смог выплатить кредит на сумму {sum}$.");
            }
            AddLog("Стадия 8: Получение кредитов.");
        }
        public void Stage8()//Получение ссуд.
        {
            //foreach (var player in Players)
            //{
            //    player.GetCredit(player.actions.FactoryCredit);
            //}
            AddLog("Стадия 9: Заявки на строительство и улучшение завода.");
        }
        public void Stage90()//Заявки на строительство. 
        {
            //foreach (var player in Players)
            //{
            //    foreach (var f in player.actions.FactoriesBuild)
            //    {
            //        player.BuildFactory(f.id, f.auto);
            //    }
            //}
        }
        public void Stage91()//Заявки на улучшение. 
        {
            //foreach (var player in Players)
            //{
            //    foreach (var f in player.actions.FactoriesUpgrade)
            //    {
            //        player.UpgradeFactory(f);
            //    }
            //}
            FinalStage();
        }
        public void FinalStage()
        {
            AddLog($"Ход № {Turn} окончен.");

            if (Turn == FINAL_TURN)
            {
                var maxCapitalPlayer = Players.OrderByDescending(p => p.Capital).FirstOrDefault();

                if (maxCapitalPlayer != null)
                {
                    foreach (var player in Players)
                    {
                        if (player != maxCapitalPlayer)
                        {
                            player.Defaulter = true;
                        }
                    }
                }
            }

            Turn++;
            foreach (var player in Players)
            {
                foreach (var factory in player.Factories)
                {
                    player.EGP += factory.GetEGP();
                    factory.CheckLevel(Turn);
                }
                player.actions.ClearActions();
            }
        }
    }
}
