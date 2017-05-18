using BudgetManager.Contracts;
using BudgetManager.Core.Models;
using Caliburn.Micro;

namespace BudgetManager.Models
{
    public class CategoryGroupModel : PropertyChangedBase, IModel
    {
        private string _name;

        public CategoryGroupModel(CategoryGroup entity)
        {
            Entity = entity;
            _name = entity.Name;
        }

        public CategoryGroup Entity { get; }

        public bool HasChanges { get; private set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                HasChanges = true;
                NotifyOfPropertyChange();
            }
        }

        public bool CanSave() => !string.IsNullOrEmpty(Name);

        public void CopyModelToEntity() => Entity.Name = Name;
    }
}