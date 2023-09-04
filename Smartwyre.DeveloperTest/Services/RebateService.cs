using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IRebateCalculatorFactory _calculatorFactory;

    public RebateService
    (
        IRebateDataStore rebateDataStore,
        IProductDataStore productDataStore,
        IRebateCalculatorFactory calculatorFactory
    )
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _calculatorFactory = calculatorFactory;
    }
    
    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        //Failures could be logged here or in a pipeline using Mediator or other technologies.
        if (_rebateDataStore.GetRebate(request.RebateIdentifier) is not Rebate rebate)
        {
            return CalculateRebateResult.Failed("Rebate not found.");
        }
        
        if (_productDataStore.GetProduct(request.ProductIdentifier) is not Product product)
        {
            return CalculateRebateResult.Failed("Product not found.");
        }
        
        //This step could be avoided using a rich domain model as shown here 
        //https://learn.microsoft.com/en-us/ef/core/modeling/inheritance
        //Doing so would move toward having a rich domain model. 
        if (_calculatorFactory.Build(rebate.Incentive) is not IRebateCalculator calculator)
        {
            return CalculateRebateResult.Failed("Incentive type has no calculator.");
        }

        //Remaining process could be refactored into a base class.
        if (!calculator.IncentiveTypeSupported(product))
        {
            return CalculateRebateResult.Failed("Product does not support incentive type.");
        }

        if (!calculator.RebateIsValid(rebate, product, request.Volume))
        {
            return CalculateRebateResult.Failed("Rebate is not valid for product and volume requested.");
        }

        var rebateAmount = calculator.CalculateAmount(rebate, product, request.Volume);
        
        _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);

        return CalculateRebateResult.Succeeded();
    }
}