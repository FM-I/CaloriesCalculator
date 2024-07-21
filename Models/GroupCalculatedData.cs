using System.Collections.ObjectModel;

namespace CaloriesCalculator.Models;

public class GroupCalculatedData : ObservableCollection<CalcucaltedTotalData>
{
    public DateOnly Date { get; set; }

    public GroupCalculatedData(DateTime date, List<CalcucaltedTotalData> data)
        : base(data)
    {
        Date = new DateOnly(date.Year, date.Month, date.Day);
    }
}
