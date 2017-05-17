using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public class BudgetViewModel : BaseView
    {
        public BudgetViewModel(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
            : base(windowManager, eventAggregator, dataService, dialogService)
        {
            BudgetSummaryComponent = new BudgetSummaryViewModel(eventAggregator, dataService);
            BudgetDetailsComponent = new BudgetDetailsViewModel(eventAggregator, dataService);
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