using System;
using System.Windows;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Models;
using Caliburn.Micro;
using Microsoft.Win32;

namespace BudgetManager.ViewModels
{
    public class ShellViewModel : Conductor<IView>, IShellView, IHandle<BudgetEvent>
    {
        public ShellViewModel(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
        {
            WindowManager = windowManager;
            EventAggregator = eventAggregator;
            DataService = dataService;
            DialogService = dialogService;

            EventAggregator.Subscribe(this);

            ViewBudget();
        }

        public bool BudgetLoaded { get; private set; }
        private IDataService DataService { get; }
        private IDialogService DialogService { get; }
        private IEventAggregator EventAggregator { get; }
        private IWindowManager WindowManager { get; }

        public override void CanClose(Action<bool> callback)
        {
            try
            {
                if (!BudgetLoaded)
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

        public void EditBudget()
        {
            if (!BudgetLoaded)
            {
                DialogService.ShowOpenBudget();

                return;
            }

            var viewModel = new BudgetDialogViewModel(EventAggregator, DataService);

            WindowManager.ShowDialog(viewModel, settings: viewModel.ViewSettings("Edit Budget"));
        }

        public void Exit() => TryClose();

        public void Handle(BudgetEvent message)
        {
            if (message == null)
                return;

            RefreshBudget();
        }

        public void OpenBudget()
        {
            try
            {
                var directory = DataService.StorageService.GetDirectory();

                var dialog = new OpenFileDialog();

                dialog.Filter = "JSON files|*.json";
                dialog.InitialDirectory = directory;
                dialog.Multiselect = false;

                if (dialog.ShowDialog() != true)
                    return;

                DataService.LoadBudget(dialog.FileName);

                BudgetLoaded = true;

                RefreshBudget();
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);
            }
        }

        public void SaveBudget()
        {
            try
            {
                if (!BudgetLoaded)
                    return;

                if (!ActiveItem.CanSaveChanges())
                {
                    DialogService.ShowInvalidData();

                    return;
                }

                DataService.SaveBudget();
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);
            }
        }

        public void ViewBudget()
            => DisplayView("Spending", new BudgetViewModel(DataService, DialogService), false);

        public void ViewCategories()
            => DisplayView("Categories", new CategoryViewModel(DataService, DialogService));

        public void ViewCategoryGroups()
            => DisplayView("Category Groups", new CategoryGroupViewModel(DataService, DialogService));

        public void ViewTransactions()
            => DisplayView("Transactions", new TransactionViewModel(DataService, DialogService));

        private void DisplayView(string displayName, IView view, bool? shouldPromptForLoadedBudget = true)
        {
            if (shouldPromptForLoadedBudget == true)
            {
                if (!BudgetLoaded)
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

        private void RefreshBudget()
        {
            var date = DataService.Budget.CreatedOn.ToString("MMMM yyyy");

            DisplayName = $"iBudget | {date} | Spending";

            ActiveItem.Load();
        }
    }
}