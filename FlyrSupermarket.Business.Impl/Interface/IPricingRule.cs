using Flyr.Infrastructure.Model;

namespace FlyrSupermarket.Business.Interface
{
    public interface IPricingRule
    {
        decimal ApplyRule(Product product, int quantity);
        bool CanApplyRule(string productCode);
    }
}