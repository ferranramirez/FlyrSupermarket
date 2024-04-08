using Flyr.Infrastructure.Model;

namespace FlyrSupermarket.Business.Contract
{
    public interface IPricingRule
    {
        decimal ApplyRule(Product product, int quantity);
        bool CanApplyRule(string productCode);
    }
}