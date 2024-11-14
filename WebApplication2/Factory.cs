using static WebApplication2.GameSettings;
namespace WebApplication2
{
    public class Factory
    {
        public int Level { get; private set; } // -1 - не построен,
                                               //  0 - строительство обычного завода,
                                               //  1 - строительство улучшенного завода,
                                               //  2 - обычный завод,
                                               //  3 - улучшение обычного завода,
                                               //  4 - улучшенный завод.

        public int TurnForNext { get; private set; }  // Ход, на котором завод получит новый уровень
        public int ESM { get; private set; }
        public bool IsCredit { get; set; } // под залогом?
        public Factory()
        {
            Level = -1;
            TurnForNext = 0;
            ESM = 0;
            IsCredit = false;
        }
        public Factory(int level)
        {
            Level = level;
            TurnForNext = 0;
            ESM = 0;
            IsCredit = false;
        }
        public bool CheckLevel(int currentTurn)
        {
            if (TurnForNext == currentTurn) switch (Level)
                {
                    case 0:
                        Level = 2;
                        return true;
                    case 1 or 3:
                        Level = 4;
                        return true;
                    default:
                        break;
                }
            return false;
        }
        public bool Build(int currentTurn, bool toAuto)
        {
            if (Level == -1) 
                switch (toAuto)
                    {
                    case false:
                        Level = 0;
                        return true;
                    case true:
                        Level = 1;
                        return true;
                }
            return false;
        }
        public bool Upgrade(int currentTurn)
        {
            switch (Level)
            {
                case 2:
                    Level = 3;
                    return true;
                default:
                    break;
            }
            return false;
        }
        public int ProcessESM(int ESM)
        {
            if (Level < 2) this.ESM = 0;
            if ((ESM > 1) && (Level == 4)) this.ESM = 2;
            else this.ESM = 1;

            return this.ESM;
        }
        public int GetEGP()
        {
            int result = this.ESM;
            this.ESM = 0;
            return result;
        }
        public int Cost => Level switch
        {
            -1 => 0,
            0 or 1 or 2 or 3 => COST_FACTORY,
            4 => COST_AUTO_FACTORY,
            _ => throw new NotImplementedException()
        };
    }
}
