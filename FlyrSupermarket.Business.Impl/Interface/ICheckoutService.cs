namespace FlyrSupermarket.Business.Interface
{
    public interface ICheckoutService
    {
        public decimal Total();
        public void Scan(string productCode);
    }
}