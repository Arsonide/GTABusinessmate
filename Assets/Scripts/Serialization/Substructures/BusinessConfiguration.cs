namespace Arsonide
{
    [System.Serializable]
    public class BusinessConfiguration
    {
        public string BusinessID;
        public float Priority;
        public string ProductName;
        public string FacilityName;
        public float ProductionAmount;
        public float ProductionMaximum;
        public float ProductionRate;
        public float SupplyAmount;
        public float SupplyMaximum;
        public float SupplyRate;
        public float SaleThreshold;
        public float ResupplyThreshold;
        public float SalePriceClose;
    }
}