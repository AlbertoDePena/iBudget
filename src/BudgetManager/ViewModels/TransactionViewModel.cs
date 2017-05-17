using System;
using System.Collections.Generic;
using System.Linq;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Core.Models;
using BudgetManager.Models;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public class TransactionViewModel : BaseView, IEditableView
    {
        public TransactionViewModel(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
            : base(windowManager, eventAggregator, dataService, dialogService)
        {
            Transactions = new BindableCollection<TransactionModel>();
            Categories = new BindableCollection<KeyValuePair<Guid?, string>>();
        }

        public BindableCollection<TransactionModel> Transactions { get; }

        public BindableCollection<KeyValuePair<Guid?, string>> Categories { get; }

        public string Total
        {
            get
            {
                var spent = Transactions.Sum(x => x.Amount).ToString("C");

                return $"Total: {spent}";
            }
        }

        public void Add()
            => Transactions.Add(new TransactionModel(new Transaction() { Date = DateTime.UtcNow }, string.Empty));

        public override bool CanSaveChanges() => Transactions.All(x => x.CanSave());

        public override void Load()
        {
            Transactions.Clear();
            Categories.Clear();

            Categories.AddRange(DataService.Categories.Select(x => new KeyValuePair<Guid?, string>(x.Id, x.Name)));

            foreach (var item in DataService.Budget.Transactions.OrderBy(x => x.Date))
            {
                var category = Categories.FirstOrDefault(x => x.Key == item.CategoryId);

                Transactions.Add(new TransactionModel(item, category.Value));
            }

            NotifyOfPropertyChange(() => Total);
        }

        public void Remove(object dataContext)
        {
            var model = dataContext as TransactionModel;

            if (model == null)
                return;

            Transactions.Remove(model);

            NotifyOfPropertyChange(() => Total);

            if (!model.Entity.Id.HasValue)
                return;

            try
            {
                DataService.DeleteTransaction(model.Entity.Id.Value);
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);
            }
        }

        protected override void Save()
        {
            foreach (var item in Transactions)
            {
                item.CopyModelToEntity();

                DataService.CreateOrUpdateTransaction(item.Entity);
            }
        }
    }
}