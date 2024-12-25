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

            var sortedPlayers = Room.Players
            .OrderByDescending(p => p.actions.RequestedESM.price) // Сортируем по цене
            .ThenByDescending(p => p.IsMainPlayer) // Главный игрок выше при одинаковой цене
            .ToList();

            // Обрабатываем заявки на ESM
            Dictionary<Player, (int count, int price)> result = new Dictionary<Player, (int, int)>();

            foreach (Player player in sortedPlayers)
            {
                if ((player.actions.RequestedESM.count < 0) || (player.actions.RequestedESM.price < 0))
                {
                    player.actions.RequestedESM = (0, 0);
                }

                if ((player.actions.RequestedESM.count > ESMCount) || (player.actions.RequestedESM.price < ESMPrice) || (player.actions.RequestedESM.price * player.actions.RequestedESM.count > player.Money))
                {
                    player.actions.RequestedESM = (0, 0);
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

            var sortedPlayers = Room.Players
            .OrderBy(p => p.actions.RequestedEGP.price) // Сортируем по цене
            .ThenByDescending(p => p.IsMainPlayer) // Главный игрок выше при одинаковой цене
            .ToList();

            // Обрабатываем заявки на ESM
            Dictionary<Player, (int count, int price)> result = new Dictionary<Player, (int, int)>();

            foreach (Player player in sortedPlayers)
            {
                if ((player.actions.RequestedEGP.count < 0) || (player.actions.RequestedEGP.price < 0))
                {
                    player.actions.RequestedEGP = (0, 0);
                }

                if ((player.actions.RequestedEGP.count > player.EGP) || (player.actions.RequestedEGP.count > EGPCount) || (player.actions.RequestedEGP.price > EGPPrice))
                {
                    player.actions.RequestedEGP = (0, 0);
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
    }
}
