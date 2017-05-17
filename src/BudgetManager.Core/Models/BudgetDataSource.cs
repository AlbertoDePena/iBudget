using BudgetManager.Core.Contracts;

namespace BudgetManager.Core.Models
{
    public class BudgetDataSource : IDataSource
    {
        public Budget Budget { get; set; }

        public string FilePath { get; set; }
    }
}