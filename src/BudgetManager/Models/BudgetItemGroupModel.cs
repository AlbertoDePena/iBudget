using System.Collections.Generic;

namespace BudgetManager.Models
{
    public class BudgetItemGroupModel : BaseItemModel
    {
        public BudgetItemGroupModel(string name, double budgeted, double spent)
            : base(name, budgeted, spent)
        {
            Items = new List<BudgetItemModel>();
        }

        public List<BudgetItemModel> Items { get; }
    }
}