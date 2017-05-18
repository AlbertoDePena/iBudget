using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public abstract class BaseComponent : PropertyChangedBase, IComponent
    {
        protected BaseComponent(IDataService dataService)
        {
            DataService = dataService;
        }

        protected IDataService DataService { get; }

        public virtual void Update()
        {
        }
    }
}