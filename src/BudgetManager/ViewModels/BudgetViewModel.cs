using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;

namespace BudgetManager.ViewModels
{
    public class BudgetViewModel : BaseView
    {
        public BudgetViewModel(IDataService dataService, IDialogService dialogService)
            : base(dataService, dialogService)
        {
            BudgetSummaryComponent = new BudgetSummaryViewModel(dataService);
            BudgetDetailsComponent = new BudgetDetailsViewModel(dataService);
        }

        public BudgetSummaryViewModel BudgetSummaryComponent { get; }

        public BudgetDetailsViewModel BudgetDetailsComponent { get; }

        public override void Load()
        {
            BudgetSummaryComponent.Update();
            BudgetDetailsComponent.Update();
        }
    }
}