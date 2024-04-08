namespace FlyrSupermarket.Business.Contract
{
    public interface ICheckoutService
    {
        public decimal Total();
        public void Scan(string productCode);
    }
}