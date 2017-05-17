using System;
using System.Collections.Generic;
using BudgetManager.Contracts;
using BudgetManager.Core.Models;
using Caliburn.Micro;

namespace BudgetManager.Models
{
    public class CategoryModel : PropertyChangedBase, IModel
    {
        private double _amount;
        private KeyValuePair<Guid?, string> _categoryGroup;
        private string _name;

        public CategoryModel(Category entity, string categoryGroupName)
        {
            Entity = entity;
            _name = entity.Name;
            _amount = entity.Amount;
            _categoryGroup = new KeyValuePair<Guid?, string>(entity.CategoryGroupId, categoryGroupName);
        }

        public double Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyOfPropertyChange();
            }
        }

        public KeyValuePair<Guid?, string> CategoryGroup
        {
            get { return _categoryGroup; }
            set
            {
                _categoryGroup = value;
                NotifyOfPropertyChange();
            }
        }

        public Category Entity { get; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        public bool CanSave()
            => !string.IsNullOrEmpty(Name) && Amount >= 0 && CategoryGroup.Key.HasValue;

        public void CopyModelToEntity()
        {
            Entity.Name = Name;
            Entity.Amount = Amount;
            Entity.CategoryGroupId = CategoryGroup.Key.GetValueOrDefault();
        }
    }
}