using System;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public abstract class BaseView : Screen, IView
    {
        protected BaseView(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
        {
            WindowManager = windowManager;
            EventAggregator = eventAggregator;
            DataService = dataService;
            DialogService = dialogService;
        }

        protected IDataService DataService { get; }
        protected IDialogService DialogService { get; }
        protected IEventAggregator EventAggregator { get; }
        protected IWindowManager WindowManager { get; }

        public override void CanClose(Action<bool> callback) => callback(SaveChanges());

        public virtual bool CanSaveChanges() => true;

        public abstract void Load();

        private bool SaveChanges()
        {
            try
            {
                if (!CanSaveChanges())
                {
                    DialogService.ShowInvalidData();

                    return false;
                }

                Save();

                Load();

                return true;
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);

                return false;
            }
        }

        protected virtual void Save()
        {
        }
    }
}