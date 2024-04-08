using FlyrSupermarket.Business.Contract;

namespace FlyrSupermarket.Business.Impl.PricingRules
{
    public class PricingRuleFactory : IPricingRuleFactory
    {
        private readonly IEnumerable<IPricingRule> _strategyList;

        public PricingRuleFactory(IEnumerable<IPricingRule> strategyList)
        {
            _strategyList = strategyList;
        }

        public IPricingRule? GetStrategy(string productCode)
        {
            return _strategyList.FirstOrDefault(str => str.CanApplyRule(productCode));
        }
    }
}
