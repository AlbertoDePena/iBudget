using System.Linq;
using BudgetManager.Core.Contracts;
using BudgetManager.Models;
using Caliburn.Micro;

namespace BudgetManager.ViewModels
{
    public class BudgetDetailsViewModel : BaseComponent
    {
        public BudgetDetailsViewModel(IEventAggregator eventAggregator, IDataService dataService)
            : base(eventAggregator, dataService)
        {
            BudgetItems = new BindableCollection<BudgetItemGroupModel>();
        }

        public BindableCollection<BudgetItemGroupModel> BudgetItems { get; }

        public override void Update()
        {
            BudgetItems.Clear();

            var transactions = DataService.Budget.Transactions;
            var categories = DataService.Categories;
            var categoryGroups = DataService.CategoryGroups;

            var data = categories.Join(categoryGroups, c => c.CategoryGroupId, cg => cg.Id, (c, cg) => new
            {
                Name = c.Name,
                GroupName = cg.Name,
                Budgeted = c.Amount,
                Spent = transactions.Where(t => t.CategoryId == c.Id).Sum(t => t.Amount)
            }).OrderBy(x => x.GroupName).ThenBy(x => x.Name).ToList();

            foreach (var item in data)
            {
                var budgeted = data.Where(x => x.GroupName.Equals(item.GroupName)).Sum(x => x.Budgeted);
                var spent = data.Where(x => x.GroupName.Equals(item.GroupName)).Sum(x => x.Spent);

                var group = BudgetItems.FirstOrDefault(x => x.Name.Equals(item.GroupName));

                if (group != null)
                {
                    group.Items.Add(new BudgetItemModel(item.Name, item.Budgeted, item.Spent));

                    continue;
                }

                group = new BudgetItemGroupModel(item.GroupName, budgeted, spent);

                group.Items.Add(new BudgetItemModel(item.Name, item.Budgeted, item.Spent));

                BudgetItems.Add(group);
            }
        }
    }
}