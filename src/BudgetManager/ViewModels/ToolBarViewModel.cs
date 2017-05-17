using System;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Enums;
using Caliburn.Micro;
using Microsoft.Win32;

namespace BudgetManager.ViewModels
{
    public class ToolBarViewModel : BaseComponent
    {
        public ToolBarViewModel(
            IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
            : base(eventAggregator, dataService)
        {
            DialogService = dialogService;
        }

        public IDialogService DialogService { get; }

        public bool BudgetLoaded { get; private set; }

        public void Exit() => EventAggregator.PublishOnUIThread(MessageEnums.ExitApp);

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

                EventAggregator.PublishOnUIThread(MessageEnums.RefreshBudget);
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);

                return;
            }
        }

        public void EditBudget() => EventAggregator.PublishOnUIThread(MessageEnums.DisplayBudgetDialog);

        public void SaveBudget()
        {
            try
            {
                if (!BudgetLoaded)
                    return;

                DataService.SaveBudget();
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);
            }
        }

        public void ViewBudget() => EventAggregator.PublishOnUIThread(MessageEnums.DisplayBudget);

        public void ViewTransactions() => EventAggregator.PublishOnUIThread(MessageEnums.DisplayTransactions);

        public void ViewCategories() => EventAggregator.PublishOnUIThread(MessageEnums.DisplayCategories);

        public void ViewCategoryGroups() => EventAggregator.PublishOnUIThread(MessageEnums.DisplayCategoryGroups);
    }
}