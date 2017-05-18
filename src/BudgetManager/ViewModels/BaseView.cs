using System;
using System.Windows;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public abstract class BaseView : Screen, IView
    {
        protected BaseView(IDataService dataService, IDialogService dialogService)
        {
            DataService = dataService;
            DialogService = dialogService;
        }

        protected IDataService DataService { get; }
        protected IDialogService DialogService { get; }

        public override void CanClose(Action<bool> callback)
        {
            var shouldClose = true;

            if (HasChanges())
            {
                shouldClose = DialogService.ShowPendingChanges() == MessageBoxResult.Yes;
            }

            callback(shouldClose);
        }

        public virtual bool CanSaveChanges() => true;

        public abstract void Load();

        public virtual bool HasChanges() => false;

        public bool SaveChanges()
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