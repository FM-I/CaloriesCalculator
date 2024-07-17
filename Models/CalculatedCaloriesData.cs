namespace CaloriesCalculator.Models;

public class CalculatedCaloriesData
{
    public List<ProductInCalculatorModel> Products { get; set; }
    public double TotalCalories => Products.Sum(x => x.TotalCalories);
    public double TotalWeight => Products.Sum(x => x.Weight);


    public CalculatedCaloriesData(List<ProductInCalculatorModel> products)
    {
        Products = [.. products];
    }
}
