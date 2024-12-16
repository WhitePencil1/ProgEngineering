using System.Numerics;

namespace WebApplication2.Models
{
    public class Room
    {
        private int _nextPlayerId = 0;
        public string Code { get; set; }
        public List<Player> Players = new();
        public Bank Bank { get; set; }
        public int MainPlayerId { get { return (Turn % Players.Count) + 1; } }
        public int Turn { get; set; }
        public int Stage { get; set; }
        public Room(string code)
        {
            Code = code;
            Bank = new Bank(this);
            Turn = 0;
        }
        public Dictionary<string, (int esm, int egp)> ResDistribution { get; set; }
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
        public void Stage1() //Постоянные издержки.
        {
            foreach (var player in Players) {
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
            foreach (var player in Players)
            {
                foreach (var f in player.actions.FactoriesProcess)
                {
                    player.ProcessESM(f.id, f.esm);
                }
            }
        }
        public void Stage5()//Продажа продукции.
        {
            Bank.ProcessEGPRequests();
        }
        public void Stage6()//Выплата ссудного процента. 
        {
            foreach (var player in Players)
            {
                foreach (var f in player.actions.FactoriesProcess)
                {
                    player.PayProcent();
                }
            }
        }
        public void Stage7()//Погашение ссуд.
        {
            foreach (var player in Players)
            {
                foreach (var f in player.actions.FactoriesProcess)
                {
                    player.PayCredits();
                }
            }
        }
        public void Stage8()//Получение ссуд.
        {
            foreach (var player in Players)
            {
                player.GetCredit(player.actions.FactoryCredit);
            }
        }
        public void Stage9()//Заявки на строительство. 
        {
            foreach (var player in Players)
            {
                foreach (var f in player.actions.FactoriesBuild)
                {
                    player.BuildFactory(f.id, f.auto);
                }
                foreach (var f in player.actions.FactoriesUpgrade)
                {
                    player.UpgradeFactory(f);
                }
            }
        }
    }
}
