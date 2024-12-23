using System;

namespace WebApplication2.Models
{
    public class Bank
    {
        //public int Id { get; set; }
        public int Level { get; set; }
        public int ActivePlayers
        {
            get
            {
                int active = 0;
                foreach (Player player in Room.Players)
                {
                    if (!player.Defaulter) active++;
                }
                return active;
            }
        }
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
        public void NewPriceLevel()
        {
            if (Level < 1 || Level > 5)
            {
                throw new ArgumentException("Уровень должен быть в диапазоне от 1 до 5.");
            }

            double randomValue = new Random().NextDouble(); // Генерируем случайное число [0, 1)
            double cumulativeProbability = 0.0;

            // Используем цикл с накоплением вероятностей
            for (int i = 0; i < 5; i++)
            {
                cumulativeProbability += transitionMatrix[Level - 1, i];
                if (randomValue <= cumulativeProbability)
                {
                    Level =  i + 1; // Уровни начинаются с 1
                    return;
                }
            }

            throw new InvalidOperationException("Invalid transition matrix or probabilities.");

        }
        public Dictionary<Player, (int, int)> ProcessESMRequests()
        {
            int eSMCount = ESMCount;

            // Сортируем игроков по ESMDesired.count
            SortPlayers(Room.Players, Room.MainPlayerId, x => x.actions.RequestedESM.price);

            // Обрабатываем заявки на ESM
            Dictionary<Player, (int count, int price)> result = new Dictionary<Player, (int, int)>();

            foreach (Player player in Room.Players)
            {
                if ((player.actions.RequestedESM.count > ESMCount) || (player.actions.RequestedESM.price < ESMPrice) || (player.actions.RequestedESM.price * player.actions.RequestedESM.count > player.Money))
                {
                    continue;
                }
                int receiveCount = Math.Min(player.actions.RequestedESM.count, eSMCount);
                int resPrice = 0;

                if (receiveCount > 0)
                {
                    player.ESM += receiveCount;
                    resPrice = receiveCount * player.actions.RequestedESM.price;
                    player.Money -= resPrice;
                    eSMCount -= receiveCount;
                }

                result.Add(player, (receiveCount, resPrice));

                if (eSMCount <= 0) break;
            }

            return result;
        }
        public Dictionary<Player, (int, int)> ProcessEGPRequests()
        {
            int eGPCount = EGPCount;

            // Сортируем игроков по ESMDesired.count
            SortPlayers(Room.Players, Room.MainPlayerId, x => x.actions.RequestedEGP.price);

            // Обрабатываем заявки на ESM
            Dictionary<Player, (int count, int price)> result = new Dictionary<Player, (int, int)>();

            foreach (Player player in Room.Players)
            {
                if ((player.actions.RequestedEGP.count > player.EGP) || (player.actions.RequestedEGP.count > EGPCount) || (player.actions.RequestedEGP.price > EGPPrice))
                {
                    continue;
                }
                int receiveCount = Math.Min(player.actions.RequestedEGP.count, eGPCount);
                int resPrice = 0;

                if (receiveCount > 0)
                {
                    player.EGP -= receiveCount;
                    resPrice = receiveCount * player.actions.RequestedEGP.price;
                    player.Money += resPrice;
                    eGPCount -= receiveCount;
                }

                result.Add(player, (receiveCount, resPrice));

                if (eGPCount <= 0) break;
            }

            return result;
        }
        private void SortPlayers(List<Player> players, int mainPlayerId, Func<Player, int> keySelector)
        {
            Random random = new Random();
            players.Sort((x, y) =>
            {
                int result = keySelector(x).CompareTo(keySelector(y));

                if (result == 0)
                {
                    if (x.Id == mainPlayerId && y.Id != mainPlayerId)
                    {
                        return -1;
                    }
                    if (y.Id == mainPlayerId && x.Id != mainPlayerId)
                    {
                        return 1;
                    }

                    return random.Next(-1, 2);
                }

                return result;
            });
        }

    }
}
