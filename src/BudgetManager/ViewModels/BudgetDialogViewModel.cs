using System.Dynamic;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Models;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public class BudgetDialogViewModel : Screen, IDialog
    {
        private string _errorMessage;
        private double _grossIncome;
        private float _tithe;

        public BudgetDialogViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            EventAggregator = eventAggregator;
            DataService = dataService;

            _grossIncome = dataService.Budget.GrossIncome;
            _tithe = dataService.Budget.Tithe;
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange();
            }
        }

        public double GrossIncome
        {
            get { return _grossIncome; }
            set
            {
                _grossIncome = value;
                NotifyOfPropertyChange();
            }
        }

        public float Tithe
        {
            get { return _tithe; }
            set
            {
                _tithe = value;
                NotifyOfPropertyChange();
            }
        }

        private IDataService DataService { get; }

        private IEventAggregator EventAggregator { get; }

        public void Save()
        {
            if (!CanSave())
                return;

            DataService.CreateOrUpdateBudget(GrossIncome, Tithe);

            EventAggregator.PublishOnUIThread(new BudgetEvent());

            TryClose();
        }

        public dynamic ViewSettings(string title)
        {
            dynamic settings = new ExpandoObject();

            settings.Title = $"iBudget | {title}";
            settings.MaxWidth = 350;
            settings.MaxHeight = 300;
            settings.MinWidth = 350;
            settings.MinHeight = 300;

            return settings;
        }

        private bool CanSave()
        {
            var valid = GrossIncome > 0 && Tithe > 0;

            ErrorMessage = valid ? string.Empty : "Gross Income / Tithe is invalid.";

            return valid;
        }
    }
}