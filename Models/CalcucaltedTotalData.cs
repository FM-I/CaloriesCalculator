namespace CaloriesCalculator.Models;

public class CalcucaltedTotalData
{
    public long Id { get; set; }
    public DateTime Date => new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Id).ToLocalTime();
    
    private double _totalCalories;
    public double TotalCalories { get => _totalCalories; set => _totalCalories = Math.Round(value, 2); }
    
    private double _totalWeight;
    public double TotalWeight { get => _totalWeight; set => _totalWeight = Math.Round(value, 2); }
}
