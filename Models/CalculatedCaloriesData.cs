namespace CaloriesCalculator.Models;

public class CalculatedCaloriesData
{
    public long Id { get; set; }
    public List<ProductInCalculatorModel> Products { get; set; }
    public double TotalCalories => Math.Round(Products.Sum(x => x.TotalCalories), 2);
    public double TotalWeight => Math.Round(Products.Sum(x => x.Weight), 2);

    public CalculatedCaloriesData(List<ProductInCalculatorModel> products)
    {
        Products = [.. products];
    }
}
