using System;
using System.Linq;
using System.Windows;
using BudgetManager.Contracts;
using BudgetManager.Core.Contracts;
using BudgetManager.Core.Models;
using BudgetManager.Models;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public class CategoryGroupViewModel : BaseView, IEditableView
    {
        public CategoryGroupViewModel(
            IWindowManager windowManager, IEventAggregator eventAggregator, IDataService dataService, IDialogService dialogService)
            : base(windowManager, eventAggregator, dataService, dialogService)
        {
            CategoryGroups = new BindableCollection<CategoryGroupModel>();
        }

        public BindableCollection<CategoryGroupModel> CategoryGroups { get; }

        public void Add()
            => CategoryGroups.Add(new CategoryGroupModel(new CategoryGroup()));

        public override bool CanSaveChanges()
        {
            var validModels = CategoryGroups.All(x => x.CanSave());

            var hasDuplicates = CategoryGroups.GroupBy(x => x.Name).Any(g => g.Count() > 1);

            return validModels && !hasDuplicates;
        }

        public override void Load()
        {
            CategoryGroups.Clear();

            foreach (var item in DataService.CategoryGroups.OrderBy(x => x.Name))
            {
                CategoryGroups.Add(new CategoryGroupModel(item));
            }
        }

        public void Remove(object dataContext)
        {
            var model = dataContext as CategoryGroupModel;

            if (model == null)
                return;

            CategoryGroups.Remove(model);

            if (model.Entity.Id.HasValue)
                return;

            try
            {
                DataService.DeleteCategoryGroup(model.Entity.Id.Value);
            }
            catch (Exception e)
            {
                DialogService.ShowException(e);
            }
        }

        protected override void Save()
        {
            foreach (var item in CategoryGroups)
            {
                item.CopyModelToEntity();

                DataService.CreateOrUpdateCategoryGroup(item.Entity);
            }
        }
    }
}