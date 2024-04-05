namespace FlyrSupermarket.Business.Impl
{
    public interface ICheckoutService
    {
        public decimal Total();
        public void Scan(string productCode);
    }
}