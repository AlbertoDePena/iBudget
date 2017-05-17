namespace BudgetManager.Models
{
    public class BudgetItemModel : BaseItemModel
    {
        public BudgetItemModel(string name, double budgeted, double spent)
            : base(name, budgeted, spent)
        {
        }
    }
}