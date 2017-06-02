using System.Linq;
using System.Windows.Media;
using BudgetManager.Core.Contracts;

namespace BudgetManager.ViewModels
{
    public class BudgetSummaryViewModel : BaseComponent
    {
        public BudgetSummaryViewModel(IDataService dataService)
            : base(dataService)
        { }

        public double Budgeted => DataService.Categories.Sum(x => x.Amount);

        public SolidColorBrush BudgetedBrush => Budgeted <= NetIncome ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Orange);

        public double GrossIncome => DataService.Budget.GrossIncome;

        public double LeftOver => NetIncome - Spent;

        public SolidColorBrush LeftOverBrush => LeftOver >= 0 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.Red);

        public double NetIncome => GrossIncome - (GrossIncome * Tithe);

        public double Spent => DataService.Budget.Transactions.Sum(x => x.Amount);

        public float Tithe => DataService.Budget.Tithe;

        public override void Update()
        {
            NotifyOfPropertyChange(() => GrossIncome);
            NotifyOfPropertyChange(() => Tithe);
            NotifyOfPropertyChange(() => NetIncome);
            NotifyOfPropertyChange(() => Budgeted);
            NotifyOfPropertyChange(() => Spent);
            NotifyOfPropertyChange(() => LeftOver);
            NotifyOfPropertyChange(() => LeftOverBrush);
            NotifyOfPropertyChange(() => BudgetedBrush);
        }
    }
}