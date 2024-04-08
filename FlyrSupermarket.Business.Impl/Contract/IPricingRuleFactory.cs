namespace FlyrSupermarket.Business.Contract
{
    public interface IPricingRuleFactory
    {
        IPricingRule? GetStrategy(string productCode);
    }
}
