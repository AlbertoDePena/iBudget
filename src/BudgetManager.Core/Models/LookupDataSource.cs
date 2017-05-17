using System.Collections.Generic;
using BudgetManager.Core.Contracts;

namespace BudgetManager.Core.Models
{
    public class LookupDataSource : IDataSource
    {
        public List<Category> Categories { get; set; } = new List<Category>();

        public List<CategoryGroup> CategoryGroups { get; set; } = new List<CategoryGroup>();

        public string FilePath { get; set; }
    }
}