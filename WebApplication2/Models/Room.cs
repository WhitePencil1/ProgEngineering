namespace WebApplication2.Models
{
    public class Room
    {
        public string Code { get; set; }
        public List<Player> Players { get; set; }
        public Bank Bank { get; set; }
        public string KingPlayerID { get; set; }
        public int Turn { get; set; }
        public Room(string code)
        {
            Code = code;
            Players = new List<Player>();
            Bank = new Bank(this);
            Turn = 0;
        }
        public Dictionary<string, (int esm, int egp)> ResDistribution { get; set; }
        public (bool success, string message, string playerId) Join(string playerName, int avatar)
        {
            Player player = new(playerName, avatar, this);
            Players.Add(player);
            return new(true, $"игрок {playerName} добавлен", player.Id);
        }
        public (bool success, string message) Leave(string playerId)
        {
            Player? player = Players.Find(item => item.Id == playerId);
            return player != null
                ? Players.Remove(player)
                    ? (true, $"игрок {playerId} удалён")
                    : (false, $"ошибка удаления игрока {playerId}")
                : (false, $"игрок {playerId} не найден");
        }
        public Player GetPlayer(string playerId)
        {
            Player? player = Players.Find(item => item.Id == playerId);
            return player;
        }
        public void NextTurn()
        {
            Turn++;
            //этап 2
            Bank.Level = Bank.GetNextPriceLevel(Bank.Level);
            ResDistribution = Bank.ProcessBids();
            //этап 3
            //этап 4
            //этап 5
            //этап 6
            //этап 7
            //этап 8
            foreach (var player in Players)
            {
                //этап 1
                player.PayTheCosts();

                foreach (var factory in player.Factories)
                {
                    factory.CheckLevel(Turn);
                }

            }
        }
    }
}
