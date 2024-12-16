namespace WebApplication2.Models
{
    public class Actions
    {
        public bool Surrend { get; set; }
        public (int count, int price) RequestedESM { get; set; }
        public (int count, int price) RequestedEGP { get; set; }
        public List<int> FactoriesUpgrade { get; set; }
        public List<(int id, bool auto)> FactoriesBuild { get; set; }
        public List<(int id, int esm)> FactoriesProcess { get; set; }
        public int FactoryCredit { get; set; }
        public Actions() {
            ClearActions();
        }
        public void ClearActions() {
            RequestedESM = (-1, -1);
            RequestedEGP = (-1, -1);
            FactoriesUpgrade = [];
            FactoriesBuild = [];
            FactoriesProcess = [];
            FactoryCredit = -1;
            Surrend = false;
        }
    }
}
