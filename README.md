# Welcome to the FLYR Supermarket Application! 👋

## ✨ Development Overview

> Here, I'll explain the decisions made during the project's development.

1. **Domain-Driven Design (DDD) Architecture**: I opted for a DDD Architecture as it efficiently handles complexity and separates business logic into distinct layers. This approach prepares the application for potential future growth.

2. **Repository Layer**: Despite Entity Framework Core already implementing the Repository Pattern, I encapsulated it within a separate Repository Layer. This decision follows DDD principles, facilitates unit testing by isolating infrastructure code, and maintains better control over database interactions.

3. **Factory Method Pattern**: Implemented a Factory Method Pattern with the `PricingRuleFactory` class to dynamically select pricing rule strategies. This abstraction enhances flexibility by enabling easy addition or modification of pricing rules without altering client code.

4. **Pricing Rule Strategies**: Utilized Pricing Rule Strategies such as `BuyOneGetOneFreeRule` and `BulkDiscountRule`. The `PricingRuleFactory` dynamically selects the appropriate strategy based on product codes, adhering to the Open/Closed Principle.

## ✨ Testing Approach

- Employed unit tests for the business layer using mocks instead of an in-memory database for stringent unit testing.
- Mocked Pricing Rules to independently test their behavior.

## Future Steps

- To enhance maintainability, consider retrieving product codes from the `appsettings` when adding new codes to existing strategies.
- Implement a custom exception to handle scenarios where a strategy lacks behavior for a specific ProductCode, preempting potential errors.
- Although unnecessary for the current single-entity scenario, consider entity mapping for potential future expansions without compromising the repository layer's cleanliness.

## Author

👤 **Ferran Ramírez Navajón**

- LinkedIn: [@ferranramirez](https://www.linkedin.com/in/ferranramirez)
- GitHub: [@ferranramirez](https://github.com/ferranramirez)

---
