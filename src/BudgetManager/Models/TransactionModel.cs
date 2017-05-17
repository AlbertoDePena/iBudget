﻿using System;
using System.Collections.Generic;
using BudgetManager.Contracts;
using BudgetManager.Core.Models;
using Caliburn.Micro;

namespace BudgetManager.Models
{
    public class TransactionModel : PropertyChangedBase, IModel
    {
        private double _amount;
        private KeyValuePair<Guid?, string> _category;
        private string _date;
        private string _name;

        public TransactionModel(Transaction entity, string categoryName)
        {
            Entity = entity;
            _name = entity.Name;
            _amount = entity.Amount;
            _date = entity.Date.ToString("MM/dd/yyyy");
            _category = new KeyValuePair<Guid?, string>(entity.CategoryId, categoryName);
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

        public KeyValuePair<Guid?, string> Category
        {
            get { return _category; }
            set
            {
                _category = value;
                NotifyOfPropertyChange();
            }
        }

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyOfPropertyChange();
            }
        }

        public Transaction Entity { get; }

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
            => !string.IsNullOrEmpty(Name) && IsValidDate() && Amount >= 0 && Category.Key.HasValue;

        public void CopyModelToEntity()
        {
            Entity.Name = Name;
            Entity.Amount = Amount;
            Entity.CategoryId = Category.Key.GetValueOrDefault();
            Entity.Date = Convert.ToDateTime(Date);
        }

        private bool IsValidDate()
        {
            if (string.IsNullOrEmpty(Date))
            {
                return false;
            }

            DateTime value;

            return DateTime.TryParse(Date, out value);
        }
    }
}