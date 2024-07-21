using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CaloriesCalculator.Models;

public class GroupCalculatedData : ObservableCollection<CalcucaltedTotalData>
{
    public DateOnly Date { get; set; }

    public double TotalCalories { get => Math.Round(this.Sum(x => x.TotalCalories), 2); }

    public GroupCalculatedData(DateTime date, List<CalcucaltedTotalData> data)
        : base(data)
    {
        Date = new DateOnly(date.Year, date.Month, date.Day);
    }

    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        base.OnCollectionChanged(e);
        OnPropertyChanged(new(nameof(TotalCalories)));
    }
}
