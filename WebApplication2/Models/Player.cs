using System.Text.Json.Serialization;
using static WebApplication2.GameSettings;

namespace WebApplication2.Models
{
    public class Player
    {
        public int Avatar { get; set; }
        public string Id { get; private set; }
        public string Name { get; set; }
        public int Capital
        {
            get
            {
                int fCost = 0;
                foreach (var f in Factories)
                {
                    fCost += f.Cost;
                }

                int credit = 0;
                foreach (var c in Credits)
                {
                    credit += c.sum;
                }
                return Money + fCost + ESM * Room.Bank.ESMPrice + EGP * Room.Bank.EGPPrice - credit;
            }
        }
        public int ESM { get; set; }
        public int EGP { get; set; }
        [JsonIgnore] public (int count, int price) ESMDesired { get; set; }
        [JsonIgnore] public (int count, int price) EGPDesired { get; set; }
        public int Money { get; set; }
        [JsonIgnore] private Room Room { get; set; }
        public List<Factory> Factories { get; set; }
        public List<(Factory factory, int sum, int turn)> Credits { get; set; }
        public bool Defaulter { get; set; }
        public Player(string name, int avatar, Room room)//ну типо
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Avatar = avatar;
            Defaulter = false;
            Factories = new List<Factory>();
            Credits = new List<(Factory factory, int summ, int turn)>();
            Room = room;
        }
        public void GiveStartKit()
        {
            Factories.Clear();
            for (int i = 0; i < START_FACTORIES; i++)
            {
                Factories.Add(new Factory(2));
            }
            for (int i = 0; i < START_MAX_FACTORIES - START_FACTORIES; i++)
            {
                Factories.Add(new Factory());
            }
            Money = START_MONEY;
            EGP = START_EGP;
            ESM = START_ESM;
        }
        public (bool success, int cost) BuildFactory(int factoryId, bool auto)
        {
            if (auto && Money > BUILD_COST_AUTO_FACTORY || !auto && Money > BUILD_COST_FACTORY)
            {
                if (Factories[factoryId].Build(Room.Turn, auto))
                {
                    int cost = auto ? BUILD_COST_AUTO_FACTORY : BUILD_COST_FACTORY;
                    Money -= cost;
                    return (true, cost);
                }
            }
            return (false, 0);
        }
        public (bool success, int cost) UpgradeFactory(int factoryId)
        {
            if (Money < UPGRADE_COST_FACTORY)
                return (false, 0);
            if (Factories[factoryId].Upgrade(Room.Turn))
            {
                Money -= UPGRADE_COST_FACTORY;
                return (true, UPGRADE_COST_FACTORY);
            }
            return (false, 0);
        }
        public void ProcessESM(int factoryId, int ESM)
        {
            this.ESM -= Factories[factoryId].ProcessESM(ESM);
        }
        public (int esm, int egp, int factory, int total, bool defaulter) PayTheCosts()
        {
            int esmCost = ESM * COST_ESM;
            int egpCost = EGP * COST_EGP;
            int factoryCost = 0;

            foreach (var factory in Factories)
            {
                factoryCost += factory.Cost;
            }

            int totalCost = esmCost + egpCost + factoryCost;
            Money -= totalCost;

            if (Money < 0) Defaulter = true;
            return (esmCost, egpCost, factoryCost, totalCost, Defaulter);
        }
        public bool BuyESM(int count, int price)
        {
            if (count > Room.Bank.ESMCount || price < Room.Bank.ESMPrice)
            {
                EGPDesired = (0, 0);
                return false;
            }
            ESMDesired = (count, price);
            return true;
        }
        public bool SellEGP(int count, int price)
        {
            if (count > Room.Bank.EGPCount || price > Room.Bank.EGPPrice)
            {
                EGPDesired = (0, 0);
                return false;
            }
            EGPDesired = (count, price);
            return true;
        }
        public (bool success, string message) Surrend()
        {
            if (!Defaulter) return (false, $"игрок {Name} уже сдался");
            Defaulter = true;
            return (true, $"игрок {Name} сдался");
        }
        public (bool success, string message) GetCredit(int factoryId, int sum)
        {
            if (Capital / 2 < sum) return (false, "недостаточно средств для заёмов");
            if (Factories[factoryId].Cost < sum) return (false, "недостаточная ценность залога");
            if (Factories[factoryId].IsCredit) return (false, "фабрика уже заложена");
            Factories[factoryId].IsCredit = true;
            Money += sum;
            Credits.Add((Factories[factoryId], sum, Room.Turn + CREDIT_TURNS));
            return (true, "кредит успешно взят");
        }
        public (bool success, int sum) PayCredits()//сейчас при невыплате - сразу поражение
        {
            int sum = 0;
            foreach (var credit in Credits)
            {
                if (credit.turn == Room.Turn)
                {
                    sum += credit.sum;
                    Money -= credit.sum;
                    if (!Defaulter)
                    {
                        if (Money >= 0)
                        {

                            credit.factory.IsCredit = false;

                        }
                        else
                        {
                            Surrend();
                        }
                    }
                }
            }

            return (!Defaulter, sum);
        }
        public (bool success, int procent) PayProcent()//сейчас при невыплате - сразу поражение
        {
            int sum = 0;
            foreach (var credit in Credits)
            {
                sum += credit.sum;
            }
            sum = sum * (CREDIT_PROCENT / 100);
            Money -= sum;
            if (Money < 0)
            {
                Surrend();
                return (false, sum);
            }
            return (true, sum);
        }
    }
}
