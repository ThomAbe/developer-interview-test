using System;
using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    /*
     * For production code I would run this as a web service for observability and resilience reasons.
     * A health check to poll against and also use OpenTelemetry for traces and metrics.
     */
    static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<IRebateCalculatorFactory, RebateCalculatorFactory>();
        services.AddSingleton<IRebateDataStore, RebateDataStore>();
        services.AddSingleton<IProductDataStore, ProductDataStore>();
        services.AddSingleton<IRebateService, RebateService>();
        
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        var rebateService = serviceProvider.GetService<IRebateService>();

        var request = new CalculateRebateRequest();

        Console.WriteLine("Choose rebate: ");
        Console.WriteLine($"1: {nameof(IncentiveType.FixedRateRebate)}");
        Console.WriteLine($"2: {nameof(IncentiveType.AmountPerUom)}");
        Console.WriteLine($"3: {nameof(IncentiveType.FixedCashAmount)}");
        Console.WriteLine("4: Missing");
        Console.WriteLine("Enter rebate option: ");
        var rebateChoice = 0;
        while (!int.TryParse(Console.ReadLine(), out rebateChoice))
        {
            Console.Write("Please enter integer value: ");
        }
        request.RebateIdentifier = rebateChoice switch
        {
            1 => nameof(IncentiveType.FixedRateRebate),
            2 => nameof(IncentiveType.AmountPerUom),
            3 => nameof(IncentiveType.FixedCashAmount),
            _ => "Missing"
        };
        Console.WriteLine($"Chosen rebate: {request.RebateIdentifier}");
        
        Console.WriteLine("Choose product: ");
        Console.WriteLine($"1: {nameof(IncentiveType.FixedRateRebate)}");
        Console.WriteLine($"2: {nameof(IncentiveType.AmountPerUom)}");
        Console.WriteLine($"3: {nameof(IncentiveType.FixedCashAmount)}");
        Console.WriteLine("4: Missing");
        Console.Write("Enter product option: ");
        var productChoice = 0;
        while (!int.TryParse(Console.ReadLine(), out productChoice))
        {
            Console.Write("Please enter integer value: ");
        }
        request.ProductIdentifier = productChoice switch
        {
            1 => nameof(IncentiveType.FixedRateRebate),
            2 => nameof(IncentiveType.AmountPerUom),
            3 => nameof(IncentiveType.FixedCashAmount),
            _ => "Missing"
        };
        Console.WriteLine($"Chosen product: {request.ProductIdentifier}");
        
        Console.WriteLine("Enter volume");
        var volume = 0m;
        while (!decimal.TryParse(Console.ReadLine(), out volume))
        {
            Console.Write("Please enter decimal value: ");
        }

        request.Volume = volume;
        
        var result = rebateService.Calculate(request);
        
        Console.WriteLine($"Success: {result.Success}");
        if (!result.Success)
        {
            Console.WriteLine($"Error reason: {result.Reason}");
        }
    }
}
