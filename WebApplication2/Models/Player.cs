using System.Text.Json.Serialization;
using static WebApplication2.GameSettings;

namespace WebApplication2.Models
{
    public class Player
    {
        public int Avatar { get; set; }
        public int Id { get; private set; }
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
        public int Money { get; set; }
        [JsonIgnore] public Room Room { get; set; }
        public List<Factory> Factories { get; set; }
        public List<(Factory factory, int sum, int turn)> Credits { get; set; }
        public bool Defaulter { get; set; }
        public Actions actions = new Actions();
        public Player(string name, int avatar, int id, Room room)//ну типо
        {
            Id = id;
            Name = name;
            Avatar = avatar;
            Defaulter = false;
            Factories = new List<Factory>();
            Credits = new List<(Factory factory, int summ, int turn)>();
            Room = room;
            GiveStartKit();
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
                //ПОТОМ УБРАТЬ ПАРАМЕТР
                Factories.Add(new Factory());
            }
            Money = START_MONEY;
            EGP = START_EGP;
            ESM = START_ESM;
        }
        public (int code, int cost) BuildFactory(int factoryId, bool auto)// -2 - уже построен, -1 - не хватает денег
        {
            if (auto && Money > BUILD_COST_AUTO_FACTORY || !auto && Money > BUILD_COST_FACTORY)
            {
                if (Factories[factoryId].Build(Room.Turn, auto))
                {
                    int cost = auto ? BUILD_COST_AUTO_FACTORY : BUILD_COST_FACTORY;
                    Money -= cost;
                    return (1, cost);
                }
                return (-2, 0);
            }
            return (-1, 0);
        }
        public (int code, int cost) UpgradeFactory(int factoryId)// -2 - не может быть улучшена в данный момент, -1 - не хватает денег
        {
            if (Money < UPGRADE_COST_FACTORY)
                return (-1, 0);
            if (Factories[factoryId].Upgrade(Room.Turn))
            {
                Money -= UPGRADE_COST_FACTORY;
                return (1, UPGRADE_COST_FACTORY);
            }
            return (-2, 0);
        }
        public int ProcessESM(int factoryId, int ESM)//-3 - не хватает ESM, -2 - не построен, -1 - нет места
        {
            if (ESM > this.ESM) return -3;
            int temp = Factories[factoryId].ProcessESM(ESM);
            this.ESM -= temp;

            switch (temp)
            {
                case 1:
                    if (Money < ESM * COST_PROCESS_ESM) return -4;
                    Money -= ESM * COST_PROCESS_ESM;
                    break;
                case 2:
                    if (Money < this.ESM * COST_AUTO_PROCESS_ESM) return -4;
                    Money -= ESM * COST_AUTO_PROCESS_ESM;
                    break;
                default:
                    break;
            }

            return temp;
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
                actions.RequestedEGP = (0, 0);
                return false;
            }
            actions.RequestedESM = (count, price);
            return true;
        }
        public bool SellEGP(int count, int price)
        {
            if (count > Room.Bank.EGPCount || price > Room.Bank.EGPPrice)
            {
                actions.RequestedEGP = (0, 0);
                return false;
            }
            actions.RequestedEGP = (count, price);
            return true;
        }
        public (bool success, string message) Surrend()
        {
            if (!Defaulter) return (false, $"игрок {Name} уже сдался");
            Defaulter = true;
            return (true, $"игрок {Name} сдался");
        }
        public (int code, string message) GetCredit(int factoryId)
        {
            if (Capital / 2 < Factories[factoryId].Cost) return (-1, "капитал слишком маленький");
            if (Factories[factoryId].IsCredit) return (-2, "фабрика уже заложена");
            Factories[factoryId].IsCredit = true;
            Factories[factoryId].TurnForCredit = CREDIT_TURNS;
            Money += Factories[factoryId].Cost;
            Credits.Add((Factories[factoryId], Factories[factoryId].Cost, Room.Turn + CREDIT_TURNS));
            return (1, "кредит успешно взят");
        }
        public (bool success, int sum) PayCredits()//сейчас при невыплате - сразу поражение
        {
            int sum = 0;
            foreach (var credit in Credits)
            {
                credit.factory.TurnForCredit--;
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
        public bool IsStageCompleted { get; set; } = false; // Флаг завершения стадии

        // Сбрасываем флаг после обработки стадии
        public void ResetStage()
        {
            IsStageCompleted = false;
        }
    }
}
