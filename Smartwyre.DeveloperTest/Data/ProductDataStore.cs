using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore : IProductDataStore
{
    public Product GetProduct(string productIdentifier)
    {
        // Access database to retrieve account, code removed for brevity 
        return productIdentifier switch
        {
            nameof(SupportedIncentiveType.FixedRateRebate) => new Product()
            {
                Id = 1,
                Identifier = nameof(SupportedIncentiveType.FixedRateRebate),
                SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                Uom = "cubic meter",
                Price = 200
            },
            nameof(SupportedIncentiveType.AmountPerUom) => new Product()
            {
                Id = 2,
                Identifier = nameof(SupportedIncentiveType.AmountPerUom),
                SupportedIncentives = SupportedIncentiveType.AmountPerUom,
                Uom = "liters",
                Price = 300
            },
            nameof(SupportedIncentiveType.FixedCashAmount) => new Product()
            {
                Id = 3,
                Identifier = nameof(SupportedIncentiveType.FixedCashAmount),
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount,
                Uom = "metric ton",
                Price = 500
            },
            _ => null
        };
    }
}
