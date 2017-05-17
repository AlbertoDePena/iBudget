using System;
using System.Windows;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Enums;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public class ShellViewModel : Conductor<IView>, IShellView, IHandle<MessageEnums>
    {
        public ShellViewModel(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
        {
            WindowManager = windowManager;
            EventAggregator = eventAggregator;
            DataService = dataService;
            DialogService = dialogService;

            ToolBarComponent = new ToolBarViewModel(eventAggregator, dataService, dialogService);

            EventAggregator.Subscribe(this);

            Handle(MessageEnums.DisplayBudget);
        }

        public ToolBarViewModel ToolBarComponent { get; }
        private IDataService DataService { get; }
        private IDialogService DialogService { get; }
        private IEventAggregator EventAggregator { get; }
        private IWindowManager WindowManager { get; }

        public override void CanClose(Action<bool> callback)
        {
            try
            {
                if (!ToolBarComponent.BudgetLoaded)
                {
                    callback(true);

                    return;
                }

                var result = DialogService.ShowSavePendingChanges();

                if (result == MessageBoxResult.Yes)
                {
                    DataService.SaveBudget();
                }

                callback(result != MessageBoxResult.Cancel);
            }
            catch (Exception e)
            {
                callback(false);

                DialogService.ShowException(e);
            }
        }

        public void Handle(MessageEnums message)
        {
            switch (message)
            {
                case MessageEnums.DisplayBudget:
                    DisplayBudgetView();
                    break;

                case MessageEnums.DisplayBudgetDialog:
                    DisplayBudgetDialog();
                    break;

                case MessageEnums.DisplayTransactions:
                    DisplayTransactionView();
                    break;

                case MessageEnums.DisplayCategories:
                    DisplayCategoryView();
                    break;

                case MessageEnums.DisplayCategoryGroups:
                    DisplayCategoryGroupView();
                    break;

                case MessageEnums.RefreshBudget:
                    OnRefreshBudget();
                    break;

                case MessageEnums.ExitApp:
                    TryClose();
                    break;
            }
        }

        private void DisplayBudgetDialog()
        {
            if (!ToolBarComponent.BudgetLoaded)
            {
                DialogService.ShowOpenBudget();

                return;
            }

            var viewModel = new BudgetDialogViewModel(EventAggregator, DataService);

            WindowManager.ShowDialog(viewModel, settings: viewModel.ViewSettings("Edit Budget"));
        }

        private void DisplayBudgetView()
            => DisplayView("Spending", new BudgetViewModel(WindowManager, EventAggregator, DataService, DialogService), false);

        private void DisplayCategoryGroupView()
            => DisplayView("Category Groups", new CategoryGroupViewModel(WindowManager, EventAggregator, DataService, DialogService));

        private void DisplayCategoryView()
            => DisplayView("Categories", new CategoryViewModel(WindowManager, EventAggregator, DataService, DialogService));

        private void DisplayTransactionView()
            => DisplayView("Transactions", new TransactionViewModel(WindowManager, EventAggregator, DataService, DialogService));

        private void DisplayView(string displayName, IView view, bool? shouldPromptForLoadedBudget = true)
        {
            if (shouldPromptForLoadedBudget == true)
            {
                if (!ToolBarComponent.BudgetLoaded)
                {
                    DialogService.ShowOpenBudget();

                    return;
                }
            }

            var date = DataService.Budget.CreatedOn.ToString("MMMM yyyy");

            DisplayName = $"iBudget | {date} | {displayName}";

            ActivateItem(view);

            view.Load();
        }

        private void OnRefreshBudget()
        {
            var date = DataService.Budget.CreatedOn.ToString("MMMM yyyy");

            DisplayName = $"iBudget | {date} | Spending";

            ActiveItem.Load();
        }
    }
}