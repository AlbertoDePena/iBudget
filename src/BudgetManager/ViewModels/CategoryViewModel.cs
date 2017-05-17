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
    public class CategoryViewModel : BaseView, IEditableView
    {
        public CategoryViewModel(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
            : base(windowManager, eventAggregator, dataService, dialogService)
        {
            Categories = new BindableCollection<CategoryModel>();
            CategoryGroups = new BindableCollection<KeyValuePair<Guid?, string>>();
        }

        public BindableCollection<CategoryModel> Categories { get; }

        public BindableCollection<KeyValuePair<Guid?, string>> CategoryGroups { get; }

        public void Add()
            => Categories.Add(new CategoryModel(new Category(), string.Empty));

        public override bool CanSaveChanges()
        {
            var validModels = Categories.All(x => x.CanSave());

            var hasDuplicates = Categories.GroupBy(x => x.Name).Any(g => g.Count() > 1);

            return validModels && !hasDuplicates;
        }

        public string Total
        {
            get
            {
                var spent = Categories.Sum(x => x.Amount).ToString("C");

                return $"Total: {spent}";
            }
        }

        public override void Load()
        {
            Categories.Clear();
            CategoryGroups.Clear();

            CategoryGroups.AddRange(DataService.CategoryGroups.Select(x => new KeyValuePair<Guid?, string>(x.Id, x.Name)));

            foreach (var item in DataService.Categories.OrderBy(x => x.Name))
            {
                var group = CategoryGroups.FirstOrDefault(x => x.Key == item.CategoryGroupId);

                Categories.Add(new CategoryModel(item, group.Value));
            }

            NotifyOfPropertyChange(() => Total);
        }

        public void Remove(object dataContext)
        {
            var model = dataContext as CategoryModel;

            if (model == null)
                return;

            Categories.Remove(model);

            NotifyOfPropertyChange(() => Total);

            if (!model.Entity.Id.HasValue)
                return;

            try
            {
                DataService.DeleteCategory(model.Entity.Id.Value);
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);
            }
        }

        protected override void Save()
        {
            foreach (var item in Categories)
            {
                item.CopyModelToEntity();

                DataService.CreateOrUpdateCategory(item.Entity);
            }
        }
    }
}