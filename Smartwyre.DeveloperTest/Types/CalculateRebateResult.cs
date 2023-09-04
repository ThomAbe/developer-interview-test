namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public bool Success { get; init; }
    public string Reason { get; init; }
    public static CalculateRebateResult Failed(string reason)
        => new ()
        {
            Success = false,
            Reason = reason
        };
    
    public static CalculateRebateResult Succeeded()
        => new ()
        {
            Success = true
        };
}
