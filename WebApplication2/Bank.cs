namespace WebApplication2
{
    public class Bank
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int ActivePlayers { get {
                int active = 0;
                foreach (Player player in Room.Players)
                {
                    if (!player.Defaulter) active++;
                }
                return active;
            } }
        private Room Room { get; set; }
        public Bank(Room room)
        {
            Room = room;
            Level = 3;
        }
        public int ESMCount => Level switch
        {
            1 => (int)(ActivePlayers * 1.0),
            2 => (int)(ActivePlayers * 1.5),
            3 => (int)(ActivePlayers * 2.0),
            4 => (int)(ActivePlayers * 2.5),
            5 => (int)(ActivePlayers * 3.0),
            _ => throw new ArgumentOutOfRangeException("Level", "Уровень должен быть от 1 до 5")
        };
        public int ESMPrice => Level switch
        {
            1 => 800,
            2 => 650,
            3 => 500,
            4 => 400,
            5 => 300,
            _ => throw new ArgumentOutOfRangeException("Level", "Уровень должен быть от 1 до 5")
        };
        public int EGPCount => Level switch
        {
            1 => (int)(ActivePlayers * 3.0),
            2 => (int)(ActivePlayers * 2.5),
            3 => (int)(ActivePlayers * 2.0),
            4 => (int)(ActivePlayers * 1.5),
            5 => (int)(ActivePlayers * 1.0),
            _ => throw new ArgumentOutOfRangeException("Level", "Уровень должен быть от 1 до 5")
        };
        public int EGPPrice => Level switch
        {
            1 => 6500,
            2 => 6000,
            3 => 5500,
            4 => 5000,
            5 => 4500,
            _ => throw new ArgumentOutOfRangeException("Level", "Уровень должен быть от 1 до 5")
        };
        private static readonly double[,] transitionMatrix = {
            { 1.0 / 3,     1.0 / 3,    1.0 / 6,    1.0 / 12,   1.0 / 12 }, // Уровень 1
            { 1.0 / 4,     1.0 / 3,    1.0 / 4,    1.0 / 12,   1.0 / 12 }, // Уровень 2
            { 1.0 / 12,    1.0 / 6,    1.0 / 3,    1.0 / 4,    1.0 / 12 }, // Уровень 3
            { 1.0 / 12,    1.0 / 12,   1.0 / 4,    1.0 / 3,    1.0 / 4  }, // Уровень 4
            { 1.0 / 12,    1.0 / 12,   1.0 / 12,   1.0 / 3,    1.0 / 3  }  // Уровень 5
        };
        public static int GetNextPriceLevel(int currentLevel)
        {
            if (currentLevel < 1 || currentLevel > 5)
            {
                throw new ArgumentException("Уровень должен быть в диапазоне от 1 до 5.");
            }

            double[] probabilities = new double[5];
            for (int i = 0; i < 5; i++)
            {
                probabilities[i] = transitionMatrix[currentLevel - 1, i];
            }

            return GetRandomLevelBasedOnProbabilities(probabilities) + 1;
        }
        private static int GetRandomLevelBasedOnProbabilities(double[] probabilities)
        {
            double randomValue = new Random().NextDouble();
            double cumulativeProbability = 0.0;

            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulativeProbability += probabilities[i];
                if (randomValue <= cumulativeProbability)
                {
                    return i;
                }
            }

            return probabilities.Length - 1; // Вернем последний уровень, если не попали в предыдущие
        }
        public Dictionary<string, (int, int)> ProcessBids()
        {
            int esmP = ESMPrice;
            int esmC = ESMCount;
            int egpP = EGPPrice;
            int egpC = EGPCount;

            Dictionary<string, int> esmCountPairs = new Dictionary<string, int>();
            Dictionary<string, int> egpCountPairs = new Dictionary<string, int>();

            Dictionary<string, (int esm, int egp)> resCountPairs = new Dictionary<string, (int, int)>();

            // Сортируем игроков по ESMDesired.count
            SortPlayers(Room.Players, Room.KingPlayerID, x => x.ESMDesired.count);

            // Обрабатываем заявки на ESM
            esmCountPairs = ProcessBidsOf( Room.Players, ref esmC, esmP, 
                player => (player.ESMDesired.count, player.ESMDesired.price),
                (player, amount) => player.ESM += amount);

            // Сортируем игроков по EGPDesired.price
            SortPlayers(Room.Players, Room.KingPlayerID, x => x.EGPDesired.price);

            // Обрабатываем заявки на EGP
            egpCountPairs = ProcessBidsOf(Room.Players, ref egpC, egpP,
                player => (player.EGPDesired.count, player.EGPDesired.price),
                (player, amount) => player.EGP += amount);

            foreach (Player player in Room.Players)
            {
                resCountPairs.Add(player.Id, (esmCountPairs[player.Id], egpCountPairs[player.Id]));
            }
            return resCountPairs;
        }
        private void SortPlayers(List<Player> players, string kingPlayerId, Func<Player, int> keySelector)
        {
            Random random = new Random();
            players.Sort((x, y) =>
            {
                int result = keySelector(x).CompareTo(keySelector(y));

                if (result == 0)
                {
                    if (x.Id == kingPlayerId && y.Id != kingPlayerId)
                    {
                        return -1;
                    }
                    if (y.Id == kingPlayerId && x.Id != kingPlayerId)
                    {
                        return 1;
                    }

                    return random.Next(-1, 2);
                }

                return result;
            });
        }
        private static Dictionary<string, int> ProcessBidsOf( List<Player> players,
                                                              ref int esmC,
                                                              int esmP,
                                                              Func<Player, (int count, int price)> selector,
                                                              Action<Player, int> updateProperty)
        {
            Dictionary<string, int> idCountPairs = new Dictionary<string, int>();

            foreach (Player player in players)
            {
                int canReceive = Math.Min(selector(player).count, esmC);
                int affordableCount = player.Money / esmP;
                int receiveCount = Math.Min(canReceive, affordableCount);

                if (receiveCount > 0)
                {
                    updateProperty(player, receiveCount); // Обновляем либо ESM, либо EGP
                    player.Money -= receiveCount * esmP;
                    esmC -= receiveCount;
                }

                idCountPairs.Add(player.Id, receiveCount);

                //if (esmC <= 0) break;
            }

            return idCountPairs;
        }


    }
}
