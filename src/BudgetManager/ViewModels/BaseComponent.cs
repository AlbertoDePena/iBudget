using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public abstract class BaseComponent : PropertyChangedBase, IComponent
    {
        protected BaseComponent(IEventAggregator eventAggregator, IDataService dataService)
        {
            EventAggregator = eventAggregator;
            DataService = dataService;
        }

        protected IEventAggregator EventAggregator { get; }

        protected IDataService DataService { get; }

        public virtual void Update()
        {
        }
    }
}